using SHUtil;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace MWTools
{
    public partial class FormTBLExport : Form
    {
        public enum eExportFileType
        {
            XML = 0,
            Json = 1,
        }

        public enum eFileStatus
        {
            None,
            Usable,
            Busy,
            NotFoundFile,
            NotFoundDirectory,
            UnAuthorizedAccess,
        }

        public class FilePathData
        {
            public int idx;
            public string file_path;
            public eFileStatus file_status;
        }

        XmlDocument mExportConfig = null;
        XmlNode mExportConfigRootNode = null;

        bool mUseEncrypt = false;
        bool mNowProgress = false;
        bool mExportRunning = false;
        bool mExportToBinary = false;
        bool mIgnoreWildcardColumn = false;
        bool mOnlySelectExportType = false;
        string mTableLoadPath = "";
        string mTableExportPath = "";
        string mEncryptPassword = "";
        eExportFileType mExportType = eExportFileType.XML;
        HashSet<string> mExportIgnoreSheetNameList = new HashSet<string>();
        List<FilePathData> mFilePathDataList = new List<FilePathData>();

        bool mDirtySave = false;

        static readonly JsonSerializerOptions s_jsonOptions = new JsonSerializerOptions()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };

        const string MESSAGE_BOX_TITLE_ERROR = "오류";
        const string MESSAGE_BOX_TITLE_NOTICE = "알림";

        const string CONFIG_ROOT_NAME = "ExportConfig";
        const string CONFIG_FIELD_TABLE_LOAD_DIR_PATH = "TableLoadDirPath";
        const string CONFIG_FIELD_EXPORT_DIR_PATH = "ExportDirPath";
        const string CONFIG_FIELD_IGNORE_SHEET_NAMES = "IgnoreSheetsNames";
        const string CONFIG_FIELD_IGNORE_WILD_CARD_COLUMNS = "IgnoreWildcardColumns";
        const string CONFIG_FIELD_SELECT_CONVERT_MODE = "SelectConvertMode";
        const string CONFIG_FIELD_EXPORT_USE_ENCRYPT = "UseExportEncrypt";
        const string CONFIG_FIELD_EXPORT_TO_BINARY = "ExportToBinary";
        const string CONFIG_FIELD_ENCRYPT_PASSWORD = "EncryptPassword";
        const string CONFIG_FIELD_ONLY_SELECT_EXPORT_TYPE = "OnlySelectExportType";

        const string FILE_EXTENSION_BINARY = ".bytes";
        const string FILE_EXTENSION_ENCRYPT = ".ens";
        const string FILE_EXTENSION_ENCRYPT_AND_BINARY = ".shoori";

        const int MIN_LEN_ENCRYPT_PASSWORD = 4;

        //----------------------------------------------------------------------------
        public FormTBLExport()
        {
            InitializeComponent();
            InitLoader();
        }

        //----------------------------------------------------------------------------
        private void InitLoader()
        {
            Reload();

            this.FormClosed += OnFormClosed;

            TableLoadPathTextBox.Leave += OnExitTableLoadPathTextBox;
            ExportTablePathTextBox.Leave += OnExitExportTablePathTextBox;
            IgnoreSheetNamesTextBox.Leave += OnExitIgnoreSheetNamesTextBox;
            textboxEncryptPassword.Leave += OnExitEncryptPasswordTextBox;
            LoadedTableListView.MouseDoubleClick += OnDoubleClickLoadedTableListView;

            tgl_xlsx_to_xml.MouseClick += OnClickedXmlMode;
            tgl_xlsx_to_json.MouseClick += OnClickedJsonMode;
            tgl_ignore_wildcard.MouseClick += OnClickedIgnoreWildcardColumns;
            tgl_use_encrypt.MouseClick += OnClickedUseEncrypt;
            tgl_convert_bin.MouseClick += OnClickedUseToBinary;
            tgl_only_select_export_type.MouseClick += OnClickedOnlySelectTypeExport;

            btn_find_table_load_path.MouseClick += OnClickedSearchPathForLoadPath;
            btn_find_export_path.MouseClick += OnClickedSearchPathForExportPath;

            ActiveControl = null;
        }

        //----------------------------------------------------------------------------
        public void UnInit()
        {
            this.FormClosed -= OnFormClosed;
            TableLoadPathTextBox.Leave -= OnExitTableLoadPathTextBox;
            ExportTablePathTextBox.Leave -= OnExitExportTablePathTextBox;
            IgnoreSheetNamesTextBox.Leave -= OnExitIgnoreSheetNamesTextBox;
            textboxEncryptPassword.Leave -= OnExitEncryptPasswordTextBox;
            LoadedTableListView.MouseDoubleClick -= OnDoubleClickLoadedTableListView;

            tgl_xlsx_to_xml.MouseClick -= OnClickedXmlMode;
            tgl_xlsx_to_json.MouseClick -= OnClickedJsonMode;
            tgl_ignore_wildcard.MouseClick -= OnClickedIgnoreWildcardColumns;
            tgl_use_encrypt.MouseClick -= OnClickedUseEncrypt;
            tgl_convert_bin.MouseClick -= OnClickedUseToBinary;
            tgl_only_select_export_type.MouseClick -= OnClickedOnlySelectTypeExport;

            btn_find_table_load_path.MouseClick -= OnClickedSearchPathForLoadPath;
            btn_find_export_path.MouseClick -= OnClickedSearchPathForExportPath;
        }

        //----------------------------------------------------------------------------
        private void Reload()
        {
            LoadConfigData();
            RefreshAllInfo();
        }

        //----------------------------------------------------------------------------
        private void LoadConfigData()
        {
            string configFilePath = GetConfigSavePath();
            string configDirPath = Path.GetDirectoryName(configFilePath);
            if (Directory.Exists(configDirPath) == false)
                Directory.CreateDirectory(configDirPath);

            mExportConfig = new XmlDocument();
            if (File.Exists(configFilePath) == false)
            {
                mExportConfig.AppendChild(mExportConfig.CreateElement(CONFIG_ROOT_NAME));
                XmlUtil.SaveXmlDocToFile(configFilePath, mExportConfig);
            }
            else
            {
                mExportConfig.Load(configFilePath);
            }

            mExportConfigRootNode = XmlUtil.GetNode(mExportConfig, CONFIG_ROOT_NAME);

            TableLoadPathTextBox.Text = GetFirstChildValue(CONFIG_FIELD_TABLE_LOAD_DIR_PATH);
            ExportTablePathTextBox.Text = GetFirstChildValue(CONFIG_FIELD_EXPORT_DIR_PATH);

            var ignoreSheetNames = GetFirstChildValue(CONFIG_FIELD_IGNORE_SHEET_NAMES);
            if (!string.IsNullOrWhiteSpace(ignoreSheetNames))
            {
                mExportIgnoreSheetNameList.Clear();
                foreach (var splitName in ignoreSheetNames.Split(','))
                {
                    if (!string.IsNullOrWhiteSpace(splitName))
                        mExportIgnoreSheetNameList.Add(splitName);
                }
                IgnoreSheetNamesTextBox.Text = ignoreSheetNames;
            }

            // Load: SelectConvertMode
            var selectConvertModeValStr = GetFirstChildValue(CONFIG_FIELD_SELECT_CONVERT_MODE);
            if (!string.IsNullOrWhiteSpace(selectConvertModeValStr))
                mExportType = (eExportFileType)Convert.ToInt32(selectConvertModeValStr);

            // Load: ExportOptions
            var useEncryptValStr = GetFirstChildValue(CONFIG_FIELD_EXPORT_USE_ENCRYPT);
            if (!string.IsNullOrWhiteSpace(useEncryptValStr))
                mUseEncrypt = Convert.ToBoolean(useEncryptValStr);

            var useToExportValStr = GetFirstChildValue(CONFIG_FIELD_EXPORT_TO_BINARY);
            if (!string.IsNullOrWhiteSpace(useToExportValStr))
                mExportToBinary = Convert.ToBoolean(useToExportValStr);

            var ignoreWildcardCol = GetFirstChildValue(CONFIG_FIELD_IGNORE_WILD_CARD_COLUMNS);
            if (!string.IsNullOrWhiteSpace(ignoreWildcardCol))
                mIgnoreWildcardColumn = Convert.ToBoolean(ignoreWildcardCol);

            var encryptPassword = GetFirstChildValue(CONFIG_FIELD_ENCRYPT_PASSWORD);
            if (!string.IsNullOrWhiteSpace(encryptPassword))
            {
                mEncryptPassword = encryptPassword;
                textboxEncryptPassword.Text = encryptPassword;
            }

            var onlySelectExportType = GetFirstChildValue(CONFIG_FIELD_ONLY_SELECT_EXPORT_TYPE);
            if (!string.IsNullOrWhiteSpace(onlySelectExportType))
                mOnlySelectExportType = Convert.ToBoolean(onlySelectExportType);
        }

        //----------------------------------------------------------------------------
        private void RefreshAllInfo()
        {
            RefreshLoadFilePath(false);
            RefreshExportPath(false);
            RefreshIgnoreSheets(false);
            RefreshEncryptPassword(false);
            RefreshForm();

            // 값 저장은 모든 처리 가장 마지막에 되도록
            SaveFormConfig();
        }

        //----------------------------------------------------------------------------
        private void RefreshLoadFilePath(bool immediatlySave = true)
        {
            // 같은 문자열이라면 저장하지 않도록 함
            if (mTableLoadPath.Equals(TableLoadPathTextBox.Text))
                return;

            mTableLoadPath = TableLoadPathTextBox.Text;
            if (!string.IsNullOrWhiteSpace(mTableLoadPath))
            {
                bool invalidPath = false;
                if (PathUtil.IsValidPath(mTableLoadPath) == false)
                {
                    invalidPath = true;
                    MessageBox.Show("입력된 경로가 올바르지 않습니다.", MESSAGE_BOX_TITLE_ERROR);
                }

                if (invalidPath == false && Directory.Exists(mTableLoadPath) == false)
                {
                    invalidPath = true;
                    MessageBox.Show("입력된 경로에 폴더가 없습니다.", MESSAGE_BOX_TITLE_ERROR);
                }

                if (invalidPath == false)
                {
                    var arr_table_file_path = Directory.GetFiles(mTableLoadPath);
                    if (arr_table_file_path.Length <= 0)
                    {
                        invalidPath = true;
                        MessageBox.Show("입력된 경로의 파일이 없습니다.", MESSAGE_BOX_TITLE_ERROR);
                    }
                }

                // 잘못된 경로가 기입되면 입력 상자를 초기화한다
                if (invalidPath)
                    mTableLoadPath = "";
            }

            TableLoadPathTextBox.Text = mTableLoadPath;

            if (immediatlySave)
                SaveFormConfig();
            else
                mDirtySave = true;
        }

        //----------------------------------------------------------------------------
        private void RefreshExportPath(bool immediatlySave = true)
        {
            // 같은 문자열이라면 저장하지 않도록 함
            if (mTableExportPath.Equals(ExportTablePathTextBox.Text))
                return;

            mTableExportPath = ExportTablePathTextBox.Text;
            if (!string.IsNullOrWhiteSpace(mTableExportPath))
            {
                if (PathUtil.IsValidPath(mTableExportPath) == false)
                {
                    mTableExportPath = "";
                    MessageBox.Show("입력된 경로가 올바르지 않습니다.", MESSAGE_BOX_TITLE_ERROR);
                }
            }

            ExportTablePathTextBox.Text = mTableExportPath;

            if (immediatlySave)
                SaveFormConfig();
            else
                mDirtySave = true;
        }

        //----------------------------------------------------------------------------
        private void RefreshIgnoreSheets(bool immediatlySave = true)
        {
            var ignoreSheetNames = IgnoreSheetNamesTextBox.Text.Split(',');
            bool isDirty = false;
            foreach (var ignoreSheetName in ignoreSheetNames)
            {
                if (string.IsNullOrWhiteSpace(ignoreSheetName))
                    continue;

                if (mExportIgnoreSheetNameList.Contains(ignoreSheetName) == false)
                {
                    isDirty = true;
                    mExportIgnoreSheetNameList.Add(ignoreSheetName);
                }
            }

            if (isDirty == false)
                return;

            if (immediatlySave)
                SaveFormConfig();
            else
                mDirtySave = true;
        }

        //----------------------------------------------------------------------------
        private void RefreshEncryptPassword(bool immediatlySave = true)
        {
            // 같은 문자열이라면 저장하지 않도록 함
            if (mEncryptPassword.Equals(textboxEncryptPassword.Text))
                return;

            mEncryptPassword = textboxEncryptPassword.Text;
            if (!string.IsNullOrWhiteSpace(mEncryptPassword) && mEncryptPassword.Length < MIN_LEN_ENCRYPT_PASSWORD)
            {
                mEncryptPassword = "";
                MessageBox.Show("암호화에 사용될 패스워드는 4글자 이상이어야 합니다", MESSAGE_BOX_TITLE_ERROR, MessageBoxButtons.OK);
            }

            textboxEncryptPassword.Text = mEncryptPassword;

            if (immediatlySave)
                SaveFormConfig();
            else
                mDirtySave = true;
        }

        //----------------------------------------------------------------------------
        private void RefreshForm()
        {
            LoadedTableListView.Items.Clear();
            mFilePathDataList.Clear();

            string loadDirPath = TableLoadPathTextBox.Text;
            if (!string.IsNullOrWhiteSpace(loadDirPath))
            {
                if (Directory.Exists(loadDirPath) == false)
                {
                    MessageBox.Show("입력된 경로에 폴더가 없습니다.", MESSAGE_BOX_TITLE_ERROR);
                }
                else
                {
                    // "*.xlsx" 패턴으로 확장자 필터링을 OS 수준에서 처리 (post-filter 불필요)
                    var arrTableFilePath = Directory.GetFiles(loadDirPath, "*.xlsx");
                    if (arrTableFilePath.Length <= 0)
                    {
                        MessageBox.Show("입력된 경로에 파일이 없습니다.", MESSAGE_BOX_TITLE_ERROR);
                    }
                    else
                    {
                        LoadedTableListView.BeginUpdate();
                        try
                        {
                            foreach (string tableFilePath in arrTableFilePath)
                            {
                                // Excel이 열어둔 임시 파일(~$로 시작) 제외
                                if (Path.GetFileName(tableFilePath).StartsWith("~$"))
                                    continue;

                                if (PathUtil.IsValidPath(tableFilePath) == false)
                                    continue;

                                var filePathData = new FilePathData();
                                filePathData.idx = mFilePathDataList.Count;
                                filePathData.file_path = tableFilePath;
                                filePathData.file_status = eFileStatus.Usable;

                                FileStream tblFileStream = null;
                                try
                                {
                                    tblFileStream = new FileInfo(tableFilePath).Open(FileMode.Open);
                                }
                                catch (FileNotFoundException)
                                {
                                    filePathData.file_status = eFileStatus.NotFoundFile;
                                }
                                catch (UnauthorizedAccessException)
                                {
                                    filePathData.file_status = eFileStatus.UnAuthorizedAccess;
                                }
                                catch (DirectoryNotFoundException)
                                {
                                    filePathData.file_status = eFileStatus.NotFoundDirectory;
                                }
                                catch (IOException)
                                {
                                    filePathData.file_status = eFileStatus.Busy;
                                }
                                finally
                                {
                                    tblFileStream?.Close();
                                }

                                mFilePathDataList.Add(filePathData);

                                var tblPathListViewItem = new ListViewItem(Path.GetFileName(filePathData.file_path));
                                var tblPathListViewSubItem = new ListViewItem.ListViewSubItem();
                                tblPathListViewSubItem.Text = filePathData.file_status.ToString();

                                if (filePathData.file_status != eFileStatus.Usable)
                                {
                                    tblPathListViewSubItem.BackColor = Color.Red;
                                    tblPathListViewSubItem.ForeColor = Color.White;
                                }

                                tblPathListViewItem.SubItems.Add(tblPathListViewSubItem);
                                LoadedTableListView.Items.Add(tblPathListViewItem);
                            }
                        }
                        finally
                        {
                            LoadedTableListView.EndUpdate();
                        }
                    }
                }
            }

            // 변환 모드 지정
            tgl_xlsx_to_xml.Checked = mExportType == eExportFileType.XML;
            tgl_xlsx_to_json.Checked = mExportType == eExportFileType.Json;

            // 변환 추가 설정 지정
            tgl_use_encrypt.Checked = mUseEncrypt;
            tgl_convert_bin.Checked = mExportToBinary;
            tgl_ignore_wildcard.Checked = mIgnoreWildcardColumn;
            tgl_only_select_export_type.Checked = mOnlySelectExportType;

            // 암호화 관련 UI 표시 여부 설정
            if (mUseEncrypt == false)
                groupBoxEncryptOption.Hide();
            else
                groupBoxEncryptOption.Show();

            // 암호화에 사용될 Password 표시
            textboxEncryptPassword.Text = mEncryptPassword;
        }

        //----------------------------------------------------------------------------
        private string GetFirstChildValue(string nodeName)
        {
            if (mExportConfig == null)
                return "";

            var node = mExportConfig.GetElementsByTagName(nodeName);
            if (node == null || node.Count <= 0 || node[0].FirstChild == null)
                return "";

            return node[0].FirstChild.InnerText;
        }

        //----------------------------------------------------------------------------
        private string GetConfigSavePath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "_config", "tbl_exporter_config.xml");
        }

        //----------------------------------------------------------------------------
        private void SaveFormConfig(bool immediatlySave = true)
        {
            if ((immediatlySave == false && mDirtySave == false) || mExportConfig == null)
                return;

            if (mTableLoadPath.Length > 0)
                SetNodeValue(CONFIG_FIELD_TABLE_LOAD_DIR_PATH, mTableLoadPath);

            if (mTableExportPath.Length > 0)
                SetNodeValue(CONFIG_FIELD_EXPORT_DIR_PATH, mTableExportPath);

            if (mExportIgnoreSheetNameList.Count > 0)
                SetNodeValue(CONFIG_FIELD_IGNORE_SHEET_NAMES, string.Join(",", mExportIgnoreSheetNameList));

            int exportValue = (int)mExportType;
            SetNodeValue(CONFIG_FIELD_SELECT_CONVERT_MODE, exportValue.ToString());
            SetNodeValue(CONFIG_FIELD_EXPORT_USE_ENCRYPT, mUseEncrypt.ToString());
            SetNodeValue(CONFIG_FIELD_EXPORT_TO_BINARY, mExportToBinary.ToString());
            SetNodeValue(CONFIG_FIELD_ENCRYPT_PASSWORD, mEncryptPassword);
            SetNodeValue(CONFIG_FIELD_IGNORE_WILD_CARD_COLUMNS, mIgnoreWildcardColumn.ToString());
            SetNodeValue(CONFIG_FIELD_ONLY_SELECT_EXPORT_TYPE, mOnlySelectExportType.ToString());

            XmlUtil.SaveXmlDocToFile(GetConfigSavePath(), mExportConfig);
            mDirtySave = false;
        }

        //----------------------------------------------------------------------------
        private void SetNodeValue(string nodeName, string nodeValue)
        {
            // 해당하는 이름의 노드가 없을 경우 노드를 새로 추가함
            if (XmlUtil.SetNodeValue(mExportConfig, nodeName, nodeValue) == false)
            {
                var newNode = XmlUtil.AddNode(mExportConfigRootNode, nodeName);
                newNode.InnerText = nodeValue;
            }
        }

        //----------------------------------------------------------------------------
        private void OnTableExportWithXml(FilePathData pathData, string exportPath)
        {
            if (pathData == null || PathUtil.IsValidPath(pathData.file_path) == false || pathData.file_status != eFileStatus.Usable)
                return;

            if (Directory.Exists(exportPath) == false)
                Directory.CreateDirectory(exportPath);

            var getXlsxDataSet = ExportUtil.LoadXlsxFile(pathData.file_path, false);
            foreach (DataTable xlsxDataTable in getXlsxDataSet.Tables)
            {
                if (string.IsNullOrEmpty(xlsxDataTable.TableName) || mExportIgnoreSheetNameList.Contains(xlsxDataTable.TableName))
                    continue;

                if (xlsxDataTable.Rows == null || xlsxDataTable.Rows.Count < 2)
                    continue;

                var exportXmlDoc = new XmlDocument();
                var exportXmlRootNode = exportXmlDoc.AppendChild(exportXmlDoc.CreateElement("DataList"));
                XmlUtil.AddAttribute(exportXmlRootNode, "data_id", xlsxDataTable.TableName);

                var fieldNameList = new List<string>(xlsxDataTable.Columns.Count);
                var fieldNameRow = xlsxDataTable.Rows[0];
                for (int fieldNameCol = 0; fieldNameCol < xlsxDataTable.Columns.Count; fieldNameCol++)
                {
                    // 데이터 컬럼 이름들 읽어옴
                    // 데이터 컬럼을 읽어올 때 유효하지 않은 컬럼 네임일 경우 공백 컬럼을 넣어주고 의도적으로 무시함
                    // 이는 데이터를 읽을 때 의도적으로 무시하기 위함, 안전하기도 하고...
                    string colName = fieldNameRow[fieldNameCol].ToString().Trim();
                    if (string.IsNullOrWhiteSpace(colName) || (mIgnoreWildcardColumn && StringUtil.ContainsSpecialOrWildcard(colName)))
                    {
                        fieldNameList.Add(string.Empty);
                        continue;
                    }

                    fieldNameList.Add(colName);
                }

                for (int row = 1; row < xlsxDataTable.Rows.Count; row++)
                {
                    int colCount = 0;
                    int emptyCount = 0;
                    XmlNode rowNode = exportXmlRootNode.OwnerDocument.CreateElement("Row");
                    for (int col = 0; col < xlsxDataTable.Columns.Count; col++)
                    {
                        if (col >= fieldNameList.Count)
                            break;

                        if (string.IsNullOrEmpty(fieldNameList[col]))
                            continue;

                        string fieldValueText = xlsxDataTable.Rows[row][col].ToString();
                        if (string.IsNullOrWhiteSpace(fieldValueText))
                            emptyCount += 1;
                        else
                            XmlUtil.AddAttribute(rowNode, fieldNameList[col], fieldValueText);

                        colCount += 1;
                    }

                    if (emptyCount < colCount)
                        exportXmlRootNode.AppendChild(rowNode);
                }

                string writePath = Path.Combine(exportPath, $"{xlsxDataTable.TableName}.xml");
                XmlUtil.SaveXmlDocToFile(writePath, exportXmlDoc);

                // "UseEncrypt" 옵션이 True이면 "OnExportFileToBinary" 함수 내부에서 암호화 함수 함께 호출
                if (mExportToBinary)
                    OnExportFileToBinary(writePath, exportPath);
                else if (mUseEncrypt)
                    OnEncryptExportFile(writePath, exportPath);

                // OnlySelectExportType 옵션이 켜져있을 때는 최종 산출물만 남기고 원본 파일을 삭제한다
                if (mOnlySelectExportType && (mExportToBinary || mUseEncrypt) && File.Exists(writePath))
                    File.Delete(writePath);
            }
        }

        //----------------------------------------------------------------------------
        private void OnTableExportWithJson(FilePathData pathData, string exportPath)
        {
            if (pathData == null || pathData.file_status != eFileStatus.Usable || PathUtil.IsValidPath(pathData.file_path) == false)
                return;

            if (Directory.Exists(exportPath) == false)
                Directory.CreateDirectory(exportPath);

            var getXlsxDataSet = ExportUtil.LoadXlsxFile(pathData.file_path, false);
            foreach (DataTable xlsxDataTable in getXlsxDataSet.Tables)
            {
                if (string.IsNullOrEmpty(xlsxDataTable.TableName) || mExportIgnoreSheetNameList.Contains(xlsxDataTable.TableName))
                    continue;

                if (xlsxDataTable.Rows == null || xlsxDataTable.Rows.Count < 2)
                    continue;

                // 데이터 컬럼 이름들 읽어옴
                // 데이터 컬럼을 읽어올 때 유효하지 않은 컬럼 네임일 경우 공백 컬럼을 넣어주고 의도적으로 무시함
                // 이는 데이터를 읽을 때 의도적으로 무시하기 위함, 안전하기도 하고...
                var fieldNameRow = xlsxDataTable.Rows[0];
                var fieldNameList = new List<string>(xlsxDataTable.Columns.Count);
                for (int fieldNameCol = 0; fieldNameCol < xlsxDataTable.Columns.Count; fieldNameCol++)
                {
                    string colName = fieldNameRow[fieldNameCol].ToString().Trim();
                    if (string.IsNullOrWhiteSpace(colName) || (mIgnoreWildcardColumn && StringUtil.ContainsSpecialOrWildcard(colName)))
                    {
                        fieldNameList.Add(string.Empty);
                        continue;
                    }

                    fieldNameList.Add(colName);
                }

                var rowDataList = new List<Dictionary<string, string>>();
                for (int row = 1; row < xlsxDataTable.Rows.Count; row++)
                {
                    var dicRowData = new Dictionary<string, string>();
                    for (int col = 0; col < xlsxDataTable.Columns.Count; col++)
                    {
                        // 기획자 실수로 컬럼이름들 필드와 데이터 필드의 길이가 맞지 않거나, 잘못된 형식의 필드가 있을 경우 해당 Column은 무시함
                        if (col >= fieldNameList.Count)
                            break;

                        if (string.IsNullOrEmpty(fieldNameList[col]))
                            continue;

                        string fieldValueText = xlsxDataTable.Rows[row][col].ToString();
                        if (!string.IsNullOrWhiteSpace(fieldValueText))
                            dicRowData.Add(fieldNameList[col], fieldValueText);
                    }

                    // 데이터가 아예 없는 행은 변환 대상에서 제외
                    if (dicRowData.Count > 0)
                        rowDataList.Add(dicRowData);
                }

                if (rowDataList.Count <= 0)
                    continue;

                string jsonText = JsonSerializer.Serialize(rowDataList, s_jsonOptions);
                if (jsonText.Length <= 0)
                    continue;

                string writePath = Path.Combine(exportPath, $"{xlsxDataTable.TableName}.json");
                File.WriteAllText(writePath, jsonText);

                // "UseEncrypt" 옵션이 True이면 "OnExportFileToBinary" 함수 내부에서 암호화 함수 함께 호출
                if (mExportToBinary)
                    OnExportFileToBinary(writePath, exportPath);
                else if (mUseEncrypt)
                    OnEncryptExportFile(writePath, exportPath);

                // OnlySelectExportType 옵션이 켜져있을 때는 최종 산출물만 남기고 원본 파일을 삭제한다
                if (mOnlySelectExportType && (mExportToBinary || mUseEncrypt) && File.Exists(writePath))
                    File.Delete(writePath);
            }
        }

        /// <summary>
        /// 변환한 파일을 이진화 및 압축, 압축 알고리즘은 LZF 기반 알고리즘 사용
        /// </summary>
        //----------------------------------------------------------------------------
        private void OnExportFileToBinary(string filePath, string exportBasePath)
        {
            if (PathUtil.IsValidPath(filePath, true) == false || PathUtil.IsValidPath(exportBasePath) == false)
                return;

            var bytesSrc = File.ReadAllBytes(filePath);
            if (bytesSrc.Length <= 0)
                return;

            var bytesCompressed = CLZF.Compress(bytesSrc);
            if (bytesCompressed.Length <= 0)
                return;

            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string savePath = Path.Combine(exportBasePath, $"{fileName}{FILE_EXTENSION_BINARY}");
            File.WriteAllBytes(savePath, bytesCompressed);

            // 암호화 사용 옵션도 켜져있을 경우 압축된 파일에 암호화를 하도록 수정, 그에 따른 전용 파일 확장명 사용
            if (mUseEncrypt)
                OnEncryptExportFile(savePath, exportBasePath, true);

            // OnlySelectExportType 옵션이 켜져있고 암호화까지 진행됐다면 중간 산출물인 .bytes 파일을 삭제
            if (mOnlySelectExportType && mUseEncrypt && File.Exists(savePath))
                File.Delete(savePath);
        }

        /// <summary>
        /// 변환한 파일을 암호화, ToBinary 옵션과 함께 사용 시 압축 진행 후 압축 파일을 대상으로 암호화 진행
        /// </summary>
        //----------------------------------------------------------------------------
        private void OnEncryptExportFile(string filePath, string exportBasePath, bool fromToBinary = false)
        {
            if (string.IsNullOrWhiteSpace(mEncryptPassword))
                return;

            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string savePath = Path.Combine(exportBasePath, $"{fileName}{(fromToBinary ? FILE_EXTENSION_ENCRYPT_AND_BINARY : FILE_EXTENSION_ENCRYPT)}");
            FileUtil.Encrypt(filePath, savePath, mEncryptPassword);
        }

        //----------------------------------------------------------------------------
        private async void OnClickAllTableExport(object sender, EventArgs e)
        {
            if (mExportRunning)
                return;

            if (PathUtil.IsValidPath(ExportTablePathTextBox.Text) == false)
            {
                MessageBox.Show("입력된 출력 경로가 올바르지 않습니다", MESSAGE_BOX_TITLE_ERROR);
                return;
            }

            if (mFilePathDataList.Count <= 0)
            {
                MessageBox.Show("변환 대상 파일이 없습니다", MESSAGE_BOX_TITLE_ERROR);
                return;
            }

            bool isExportAll = false;
            switch (mExportType)
            {
                case eExportFileType.XML:
                    isExportAll = MessageBox.Show("등록된 모든 테이블들을 XML로 변환하시겠습니까?", MESSAGE_BOX_TITLE_NOTICE, MessageBoxButtons.OKCancel) == DialogResult.OK;
                    break;

                case eExportFileType.Json:
                    isExportAll = MessageBox.Show("등록된 모든 테이블들을 Json으로 변환하시겠습니까?", MESSAGE_BOX_TITLE_NOTICE, MessageBoxButtons.OKCancel) == DialogResult.OK;
                    break;
            }

            if (isExportAll == false)
                return;

            await RunExportAsync(mFilePathDataList.ToArray());
            MessageBox.Show("모든 테이블의 변환 작업이 완료되었습니다", MESSAGE_BOX_TITLE_NOTICE, MessageBoxButtons.OK);
        }

        //----------------------------------------------------------------------------
        private async Task RunExportAsync(FilePathData[] targets)
        {
            mExportRunning = true;
            this.Enabled = false;
            progressBarExport.Value = 0;
            progressBarExport.Maximum = targets.Length;
            progressBarExport.Visible = true;
            labelExportStatus.Visible = true;

            var exportType = mExportType;
            try
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    var pathData = targets[i];
                    labelExportStatus.Text = $"변환 중... ({i + 1}/{targets.Length}) {Path.GetFileName(pathData.file_path)}";

                    var exportPath = ExportTablePathTextBox.Text;
                    try
                    {
                        await Task.Run(() =>
                        {
                            switch (exportType)
                            {
                                case eExportFileType.XML:
                                    OnTableExportWithXml(pathData, exportPath);
                                    break;
                                case eExportFileType.Json:
                                    OnTableExportWithJson(pathData, exportPath);
                                    break;
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        SHLog.LogError($"[TBL_Exporter] 변환 실패: {pathData.file_path} - {ex.Message}");
                        MessageBox.Show($"변환 중 오류 발생:\n{Path.GetFileName(pathData.file_path)}\n{ex.Message}", MESSAGE_BOX_TITLE_ERROR);
                    }

                    progressBarExport.Value = i + 1;
                }
            }
            finally
            {
                progressBarExport.Visible = false;
                labelExportStatus.Visible = false;
                this.Enabled = true;
                mExportRunning = false;
            }
        }

        //----------------------------------------------------------------------------
        private void OnClickClose(object sender, EventArgs e)
        {
            SaveFormConfig();
            UnInit();
            Application.Exit();
        }

        //----------------------------------------------------------------------------
        private void OnClickRefresh(object sender, EventArgs e)
        {
            RefreshAllInfo();
        }

        //----------------------------------------------------------------------------
        private void OnExitTableLoadPathTextBox(object sender, EventArgs e)
        {
            if (mNowProgress)
                return;

            try
            {
                mNowProgress = true;
                RefreshLoadFilePath();
            }
            finally
            {
                mNowProgress = false;
            }

            RefreshForm();
        }

        //----------------------------------------------------------------------------
        private void OnExitExportTablePathTextBox(object sender, EventArgs e)
        {
            if (mNowProgress)
                return;

            try
            {
                mNowProgress = true;
                RefreshExportPath();
            }
            finally
            {
                mNowProgress = false;
            }
        }

        //----------------------------------------------------------------------------
        private void OnExitIgnoreSheetNamesTextBox(object sender, EventArgs e)
        {
            if (mNowProgress)
                return;

            try
            {
                mNowProgress = true;
                RefreshIgnoreSheets();
            }
            finally
            {
                mNowProgress = false;
            }
        }

        private void OnExitEncryptPasswordTextBox(object sender, EventArgs e)
        {
            if (mNowProgress)
                return;

            try
            {
                mNowProgress = true;
                RefreshEncryptPassword();
            }
            finally
            {
                mNowProgress = false;
            }
        }

        //----------------------------------------------------------------------------
        private async void OnDoubleClickLoadedTableListView(object sender, MouseEventArgs e)
        {
            if (mExportRunning)
                return;

            if (e.Button.Equals(MouseButtons.Left) && LoadedTableListView.SelectedItems.Count > 0)
            {
                if (PathUtil.IsValidPath(mTableExportPath) == false)
                {
                    MessageBox.Show("Export 경로가 비어있거나 올바르지 않습니다", MESSAGE_BOX_TITLE_ERROR, MessageBoxButtons.OK);
                    return;
                }

                var selectedPathData = mFilePathDataList.Find(a => a.idx == LoadedTableListView.SelectedItems[0].Index);
                if (selectedPathData != null)
                {
                    bool runExport = false;
                    switch (mExportType)
                    {
                        case eExportFileType.XML:
                            runExport = MessageBox.Show("선택한 테이블을 XML로 변환하시겠습니까?", MESSAGE_BOX_TITLE_NOTICE, MessageBoxButtons.OKCancel) == DialogResult.OK;
                            break;

                        case eExportFileType.Json:
                            runExport = MessageBox.Show("선택한 테이블을 Json으로 변환하시겠습니까?", MESSAGE_BOX_TITLE_NOTICE, MessageBoxButtons.OKCancel) == DialogResult.OK;
                            break;
                    }

                    if (runExport == false)
                        return;

                    await RunExportAsync(new[] { selectedPathData });
                    MessageBox.Show("Table 데이터 변환이 완료되었습니다", MESSAGE_BOX_TITLE_NOTICE, MessageBoxButtons.OK);
                }
            }
        }

        //----------------------------------------------------------------------------
        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            SaveFormConfig();
            UnInit();
        }

        //----------------------------------------------------------------------------
        private void OnClickedXmlMode(object sender, MouseEventArgs e)
        {
            if (e == null || e.Button.Equals(MouseButtons.Left) == false)
                return;

            if (tgl_xlsx_to_xml.Checked == false)
            {
                mExportType = eExportFileType.XML;
                tgl_xlsx_to_xml.Checked = true;
                tgl_xlsx_to_json.Checked = false;
                SaveFormConfig();
            }
        }

        //----------------------------------------------------------------------------
        private void OnClickedJsonMode(object sender, MouseEventArgs e)
        {
            if (e == null || e.Button.Equals(MouseButtons.Left) == false)
                return;

            if (tgl_xlsx_to_json.Checked == false)
            {
                mExportType = eExportFileType.Json;
                tgl_xlsx_to_xml.Checked = false;
                tgl_xlsx_to_json.Checked = true;
                SaveFormConfig();
            }
        }

        //----------------------------------------------------------------------------
        private void OnClickedIgnoreWildcardColumns(object sender, MouseEventArgs e)
        {
            if (e == null || e.Button.Equals(MouseButtons.Left) == false)
                return;

            tgl_ignore_wildcard.Checked = !tgl_ignore_wildcard.Checked;
            mIgnoreWildcardColumn = tgl_ignore_wildcard.Checked;
            SaveFormConfig();
        }

        //----------------------------------------------------------------------------
        private void OnClickedOnlySelectTypeExport(object sender, MouseEventArgs e)
        {
            if (e == null || e.Button.Equals(MouseButtons.Left) == false)
                return;

            tgl_only_select_export_type.Checked = !tgl_only_select_export_type.Checked;
            mOnlySelectExportType = tgl_only_select_export_type.Checked;
            SaveFormConfig();
        }

        //----------------------------------------------------------------------------
        private void OnClickedSearchPathForLoadPath(object sender, MouseEventArgs e)
        {
            if (e == null || e.Button.Equals(MouseButtons.Left) == false)
                return;

            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "테이블을 불러올 폴더를 지정하세요";
                dlg.ShowNewFolderButton = true;

                var result = dlg.ShowDialog(this);
                if (result != DialogResult.OK || PathUtil.IsValidPath(dlg.SelectedPath) == false)
                {
                    MessageBox.Show("지정한 경로가 올바르지 않습니다", MESSAGE_BOX_TITLE_ERROR);
                    return;
                }

                TableLoadPathTextBox.Text = dlg.SelectedPath;
                RefreshLoadFilePath();
                RefreshForm();
            }
        }

        //----------------------------------------------------------------------------
        private void OnClickedSearchPathForExportPath(object sender, MouseEventArgs e)
        {
            if (e == null || e.Button.Equals(MouseButtons.Left) == false)
                return;

            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "변환한 테이블 파일들을 저장할 폴더를 지정하세요";
                dlg.ShowNewFolderButton = true;

                var result = dlg.ShowDialog(this);
                if (result != DialogResult.OK || PathUtil.IsValidPath(dlg.SelectedPath) == false)
                {
                    MessageBox.Show("지정한 경로가 올바르지 않습니다", MESSAGE_BOX_TITLE_ERROR);
                    return;
                }

                ExportTablePathTextBox.Text = dlg.SelectedPath;
                RefreshExportPath();
            }
        }

        //----------------------------------------------------------------------------
        private void OnClickedUseEncrypt(object sender, MouseEventArgs e)
        {
            if (e == null || e.Button.Equals(MouseButtons.Left) == false)
                return;

            tgl_use_encrypt.Checked = !tgl_use_encrypt.Checked;
            mUseEncrypt = tgl_use_encrypt.Checked;
            if (mUseEncrypt == false)
                groupBoxEncryptOption.Hide();
            else
                groupBoxEncryptOption.Show();

            SaveFormConfig();
        }

        //----------------------------------------------------------------------------
        private void OnClickedUseToBinary(object sender, MouseEventArgs e)
        {
            if (e == null || e.Button.Equals(MouseButtons.Left) == false)
                return;

            tgl_convert_bin.Checked = !tgl_convert_bin.Checked;
            mExportToBinary = tgl_convert_bin.Checked;
            SaveFormConfig();
        }

        //----------------------------------------------------------------------------
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            // 빈공간 터치 시 현재 ActiveControl에 대한 Leave 이벤트 발생 시킴
            var target = GetChildAtPoint(e.Location);
            if (target == null)
            {
                ActiveControl = null;
            }
        }
    }
}
