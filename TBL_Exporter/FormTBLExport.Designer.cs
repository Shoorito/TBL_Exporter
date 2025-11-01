namespace MWTools
{
    partial class FormTBLExport
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTBLExport));
            this.TableLoadPathTextBox = new System.Windows.Forms.TextBox();
            this.ExportAllTabelBtn = new System.Windows.Forms.Button();
            this.RefreshBtn = new System.Windows.Forms.Button();
            this.LoadedTableListView = new System.Windows.Forms.ListView();
            this.FileNameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FileStatusHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CloseBtn = new System.Windows.Forms.Button();
            this.ExportTablePathTextBox = new System.Windows.Forms.TextBox();
            this.LoadedFileListGroupBox = new System.Windows.Forms.GroupBox();
            this.IgnoreSheetNamesGroupBox = new System.Windows.Forms.GroupBox();
            this.IgnoreSheetNamesTextBox = new System.Windows.Forms.TextBox();
            this.TableLoadPathGroupBox = new System.Windows.Forms.GroupBox();
            this.btn_find_table_load_path = new System.Windows.Forms.Button();
            this.ExportTablePathGroupBox = new System.Windows.Forms.GroupBox();
            this.btn_find_export_path = new System.Windows.Forms.Button();
            this.tgl_ignore_wildcard = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tgl_xlsx_to_json = new System.Windows.Forms.CheckBox();
            this.tgl_xlsx_to_xml = new System.Windows.Forms.CheckBox();
            this.groupBoxExportOptions = new System.Windows.Forms.GroupBox();
            this.tgl_convert_bin = new System.Windows.Forms.CheckBox();
            this.tgl_use_encrypt = new System.Windows.Forms.CheckBox();
            this.groupBoxEncryptOption = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textboxEncryptPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LoadedFileListGroupBox.SuspendLayout();
            this.IgnoreSheetNamesGroupBox.SuspendLayout();
            this.TableLoadPathGroupBox.SuspendLayout();
            this.ExportTablePathGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBoxExportOptions.SuspendLayout();
            this.groupBoxEncryptOption.SuspendLayout();
            this.SuspendLayout();
            // 
            // TableLoadPathTextBox
            // 
            this.TableLoadPathTextBox.Location = new System.Drawing.Point(4, 13);
            this.TableLoadPathTextBox.Name = "TableLoadPathTextBox";
            this.TableLoadPathTextBox.Size = new System.Drawing.Size(514, 21);
            this.TableLoadPathTextBox.TabIndex = 0;
            // 
            // ExportAllTabelBtn
            // 
            this.ExportAllTabelBtn.Font = new System.Drawing.Font("굴림", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ExportAllTabelBtn.Location = new System.Drawing.Point(546, 388);
            this.ExportAllTabelBtn.Name = "ExportAllTabelBtn";
            this.ExportAllTabelBtn.Size = new System.Drawing.Size(242, 50);
            this.ExportAllTabelBtn.TabIndex = 4;
            this.ExportAllTabelBtn.Text = "ExportAllTable";
            this.ExportAllTabelBtn.UseVisualStyleBackColor = true;
            this.ExportAllTabelBtn.Click += new System.EventHandler(this.OnClickAllTableExport);
            // 
            // RefreshBtn
            // 
            this.RefreshBtn.Font = new System.Drawing.Font("굴림", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.RefreshBtn.Location = new System.Drawing.Point(546, 332);
            this.RefreshBtn.Name = "RefreshBtn";
            this.RefreshBtn.Size = new System.Drawing.Size(242, 50);
            this.RefreshBtn.TabIndex = 5;
            this.RefreshBtn.Text = "Refresh";
            this.RefreshBtn.UseVisualStyleBackColor = true;
            this.RefreshBtn.Click += new System.EventHandler(this.OnClickRefresh);
            // 
            // LoadedTableListView
            // 
            this.LoadedTableListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FileNameHeader,
            this.FileStatusHeader});
            this.LoadedTableListView.FullRowSelect = true;
            this.LoadedTableListView.GridLines = true;
            this.LoadedTableListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.LoadedTableListView.HideSelection = false;
            this.LoadedTableListView.Location = new System.Drawing.Point(5, 16);
            this.LoadedTableListView.MultiSelect = false;
            this.LoadedTableListView.Name = "LoadedTableListView";
            this.LoadedTableListView.Scrollable = false;
            this.LoadedTableListView.Size = new System.Drawing.Size(374, 250);
            this.LoadedTableListView.TabIndex = 0;
            this.LoadedTableListView.UseCompatibleStateImageBehavior = false;
            this.LoadedTableListView.View = System.Windows.Forms.View.Details;
            // 
            // FileNameHeader
            // 
            this.FileNameHeader.Text = "File Name";
            this.FileNameHeader.Width = 270;
            // 
            // FileStatusHeader
            // 
            this.FileStatusHeader.Text = "File Status";
            this.FileStatusHeader.Width = 105;
            // 
            // CloseBtn
            // 
            this.CloseBtn.Font = new System.Drawing.Font("굴림", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.CloseBtn.Location = new System.Drawing.Point(546, 276);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(242, 50);
            this.CloseBtn.TabIndex = 6;
            this.CloseBtn.Text = "Close";
            this.CloseBtn.UseVisualStyleBackColor = true;
            this.CloseBtn.Click += new System.EventHandler(this.OnClickClose);
            // 
            // ExportTablePathTextBox
            // 
            this.ExportTablePathTextBox.Location = new System.Drawing.Point(4, 13);
            this.ExportTablePathTextBox.Name = "ExportTablePathTextBox";
            this.ExportTablePathTextBox.Size = new System.Drawing.Size(514, 21);
            this.ExportTablePathTextBox.TabIndex = 8;
            // 
            // LoadedFileListGroupBox
            // 
            this.LoadedFileListGroupBox.Controls.Add(this.LoadedTableListView);
            this.LoadedFileListGroupBox.Location = new System.Drawing.Point(3, 175);
            this.LoadedFileListGroupBox.Name = "LoadedFileListGroupBox";
            this.LoadedFileListGroupBox.Size = new System.Drawing.Size(384, 272);
            this.LoadedFileListGroupBox.TabIndex = 10;
            this.LoadedFileListGroupBox.TabStop = false;
            this.LoadedFileListGroupBox.Text = "Loaded File List";
            // 
            // IgnoreSheetNamesGroupBox
            // 
            this.IgnoreSheetNamesGroupBox.Controls.Add(this.IgnoreSheetNamesTextBox);
            this.IgnoreSheetNamesGroupBox.Location = new System.Drawing.Point(2, 86);
            this.IgnoreSheetNamesGroupBox.Name = "IgnoreSheetNamesGroupBox";
            this.IgnoreSheetNamesGroupBox.Size = new System.Drawing.Size(548, 40);
            this.IgnoreSheetNamesGroupBox.TabIndex = 11;
            this.IgnoreSheetNamesGroupBox.TabStop = false;
            this.IgnoreSheetNamesGroupBox.Text = "IgnoreSheetsNames";
            // 
            // IgnoreSheetNamesTextBox
            // 
            this.IgnoreSheetNamesTextBox.Location = new System.Drawing.Point(4, 13);
            this.IgnoreSheetNamesTextBox.Name = "IgnoreSheetNamesTextBox";
            this.IgnoreSheetNamesTextBox.Size = new System.Drawing.Size(540, 21);
            this.IgnoreSheetNamesTextBox.TabIndex = 9;
            // 
            // TableLoadPathGroupBox
            // 
            this.TableLoadPathGroupBox.Controls.Add(this.btn_find_table_load_path);
            this.TableLoadPathGroupBox.Controls.Add(this.TableLoadPathTextBox);
            this.TableLoadPathGroupBox.Location = new System.Drawing.Point(3, 3);
            this.TableLoadPathGroupBox.Name = "TableLoadPathGroupBox";
            this.TableLoadPathGroupBox.Size = new System.Drawing.Size(548, 40);
            this.TableLoadPathGroupBox.TabIndex = 12;
            this.TableLoadPathGroupBox.TabStop = false;
            this.TableLoadPathGroupBox.Text = "TableLoadPath";
            // 
            // btn_find_table_load_path
            // 
            this.btn_find_table_load_path.Location = new System.Drawing.Point(519, 12);
            this.btn_find_table_load_path.Name = "btn_find_table_load_path";
            this.btn_find_table_load_path.Size = new System.Drawing.Size(27, 23);
            this.btn_find_table_load_path.TabIndex = 19;
            this.btn_find_table_load_path.Text = "...";
            this.btn_find_table_load_path.UseVisualStyleBackColor = true;
            // 
            // ExportTablePathGroupBox
            // 
            this.ExportTablePathGroupBox.Controls.Add(this.btn_find_export_path);
            this.ExportTablePathGroupBox.Controls.Add(this.ExportTablePathTextBox);
            this.ExportTablePathGroupBox.Location = new System.Drawing.Point(2, 45);
            this.ExportTablePathGroupBox.Name = "ExportTablePathGroupBox";
            this.ExportTablePathGroupBox.Size = new System.Drawing.Size(548, 40);
            this.ExportTablePathGroupBox.TabIndex = 13;
            this.ExportTablePathGroupBox.TabStop = false;
            this.ExportTablePathGroupBox.Text = "ExportTablePath";
            // 
            // btn_find_export_path
            // 
            this.btn_find_export_path.Location = new System.Drawing.Point(519, 12);
            this.btn_find_export_path.Name = "btn_find_export_path";
            this.btn_find_export_path.Size = new System.Drawing.Size(27, 23);
            this.btn_find_export_path.TabIndex = 20;
            this.btn_find_export_path.Text = "...";
            this.btn_find_export_path.UseVisualStyleBackColor = true;
            // 
            // tgl_ignore_wildcard
            // 
            this.tgl_ignore_wildcard.AutoCheck = false;
            this.tgl_ignore_wildcard.AutoSize = true;
            this.tgl_ignore_wildcard.Location = new System.Drawing.Point(556, 101);
            this.tgl_ignore_wildcard.Name = "tgl_ignore_wildcard";
            this.tgl_ignore_wildcard.Size = new System.Drawing.Size(158, 16);
            this.tgl_ignore_wildcard.TabIndex = 14;
            this.tgl_ignore_wildcard.Text = "IgnoreWildcardColumns";
            this.tgl_ignore_wildcard.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tgl_xlsx_to_json);
            this.groupBox1.Controls.Add(this.tgl_xlsx_to_xml);
            this.groupBox1.Location = new System.Drawing.Point(3, 132);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(196, 40);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SelectConvertMode";
            // 
            // tgl_xlsx_to_json
            // 
            this.tgl_xlsx_to_json.AutoCheck = false;
            this.tgl_xlsx_to_json.AutoSize = true;
            this.tgl_xlsx_to_json.Location = new System.Drawing.Point(101, 17);
            this.tgl_xlsx_to_json.Name = "tgl_xlsx_to_json";
            this.tgl_xlsx_to_json.Size = new System.Drawing.Size(94, 16);
            this.tgl_xlsx_to_json.TabIndex = 17;
            this.tgl_xlsx_to_json.Text = ".xlsxToJson";
            this.tgl_xlsx_to_json.UseVisualStyleBackColor = true;
            // 
            // tgl_xlsx_to_xml
            // 
            this.tgl_xlsx_to_xml.AutoCheck = false;
            this.tgl_xlsx_to_xml.AutoSize = true;
            this.tgl_xlsx_to_xml.Location = new System.Drawing.Point(8, 17);
            this.tgl_xlsx_to_xml.Name = "tgl_xlsx_to_xml";
            this.tgl_xlsx_to_xml.Size = new System.Drawing.Size(89, 16);
            this.tgl_xlsx_to_xml.TabIndex = 16;
            this.tgl_xlsx_to_xml.Text = ".xlsxToXml";
            this.tgl_xlsx_to_xml.UseVisualStyleBackColor = true;
            // 
            // groupBoxExportOptions
            // 
            this.groupBoxExportOptions.Controls.Add(this.tgl_convert_bin);
            this.groupBoxExportOptions.Controls.Add(this.tgl_use_encrypt);
            this.groupBoxExportOptions.Location = new System.Drawing.Point(205, 132);
            this.groupBoxExportOptions.Name = "groupBoxExportOptions";
            this.groupBoxExportOptions.Size = new System.Drawing.Size(182, 40);
            this.groupBoxExportOptions.TabIndex = 18;
            this.groupBoxExportOptions.TabStop = false;
            this.groupBoxExportOptions.Text = "ExportOptions";
            // 
            // tgl_convert_bin
            // 
            this.tgl_convert_bin.AutoCheck = false;
            this.tgl_convert_bin.AutoSize = true;
            this.tgl_convert_bin.Location = new System.Drawing.Point(101, 17);
            this.tgl_convert_bin.Name = "tgl_convert_bin";
            this.tgl_convert_bin.Size = new System.Drawing.Size(75, 16);
            this.tgl_convert_bin.TabIndex = 17;
            this.tgl_convert_bin.Text = "ToBinary";
            this.tgl_convert_bin.UseVisualStyleBackColor = true;
            // 
            // tgl_use_encrypt
            // 
            this.tgl_use_encrypt.AutoCheck = false;
            this.tgl_use_encrypt.AutoSize = true;
            this.tgl_use_encrypt.Location = new System.Drawing.Point(8, 17);
            this.tgl_use_encrypt.Name = "tgl_use_encrypt";
            this.tgl_use_encrypt.Size = new System.Drawing.Size(89, 16);
            this.tgl_use_encrypt.TabIndex = 16;
            this.tgl_use_encrypt.Text = "UseEncrypt";
            this.tgl_use_encrypt.UseVisualStyleBackColor = true;
            // 
            // groupBoxEncryptOption
            // 
            this.groupBoxEncryptOption.AutoSize = true;
            this.groupBoxEncryptOption.BackColor = System.Drawing.SystemColors.Control;
            this.groupBoxEncryptOption.Controls.Add(this.label3);
            this.groupBoxEncryptOption.Controls.Add(this.label2);
            this.groupBoxEncryptOption.Controls.Add(this.textboxEncryptPassword);
            this.groupBoxEncryptOption.Controls.Add(this.label1);
            this.groupBoxEncryptOption.Location = new System.Drawing.Point(394, 132);
            this.groupBoxEncryptOption.Name = "groupBoxEncryptOption";
            this.groupBoxEncryptOption.Size = new System.Drawing.Size(394, 88);
            this.groupBoxEncryptOption.TabIndex = 19;
            this.groupBoxEncryptOption.TabStop = false;
            this.groupBoxEncryptOption.Text = "EncryptOption";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(6, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(361, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "[!] 암호화 시 사용된 비밀번호는 Config 파일에 저장됩니다";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(6, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(381, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "[!] 암호화 옵션 사용 시 위 비밀번호를 잊지 않도록 주의하세요";
            // 
            // textboxEncryptPassword
            // 
            this.textboxEncryptPassword.Location = new System.Drawing.Point(69, 14);
            this.textboxEncryptPassword.Name = "textboxEncryptPassword";
            this.textboxEncryptPassword.Size = new System.Drawing.Size(318, 21);
            this.textboxEncryptPassword.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Password";
            // 
            // FormTBLExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBoxEncryptOption);
            this.Controls.Add(this.groupBoxExportOptions);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tgl_ignore_wildcard);
            this.Controls.Add(this.ExportTablePathGroupBox);
            this.Controls.Add(this.TableLoadPathGroupBox);
            this.Controls.Add(this.IgnoreSheetNamesGroupBox);
            this.Controls.Add(this.LoadedFileListGroupBox);
            this.Controls.Add(this.CloseBtn);
            this.Controls.Add(this.RefreshBtn);
            this.Controls.Add(this.ExportAllTabelBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTBLExport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Shoori\'s TableExporter";
            this.LoadedFileListGroupBox.ResumeLayout(false);
            this.IgnoreSheetNamesGroupBox.ResumeLayout(false);
            this.IgnoreSheetNamesGroupBox.PerformLayout();
            this.TableLoadPathGroupBox.ResumeLayout(false);
            this.TableLoadPathGroupBox.PerformLayout();
            this.ExportTablePathGroupBox.ResumeLayout(false);
            this.ExportTablePathGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxExportOptions.ResumeLayout(false);
            this.groupBoxExportOptions.PerformLayout();
            this.groupBoxEncryptOption.ResumeLayout(false);
            this.groupBoxEncryptOption.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TableLoadPathTextBox;
        private System.Windows.Forms.Button ExportAllTabelBtn;
        private System.Windows.Forms.Button RefreshBtn;
        private System.Windows.Forms.ListView LoadedTableListView;
        private System.Windows.Forms.ColumnHeader FileNameHeader;
        private System.Windows.Forms.ColumnHeader FileStatusHeader;
        private System.Windows.Forms.Button CloseBtn;
        private System.Windows.Forms.TextBox ExportTablePathTextBox;
        private System.Windows.Forms.GroupBox LoadedFileListGroupBox;
        private System.Windows.Forms.GroupBox IgnoreSheetNamesGroupBox;
        private System.Windows.Forms.GroupBox TableLoadPathGroupBox;
        private System.Windows.Forms.GroupBox ExportTablePathGroupBox;
        private System.Windows.Forms.TextBox IgnoreSheetNamesTextBox;
        private System.Windows.Forms.CheckBox tgl_ignore_wildcard;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox tgl_xlsx_to_xml;
        private System.Windows.Forms.CheckBox tgl_xlsx_to_json;
        private System.Windows.Forms.GroupBox groupBoxExportOptions;
        private System.Windows.Forms.CheckBox tgl_convert_bin;
        private System.Windows.Forms.CheckBox tgl_use_encrypt;
        private System.Windows.Forms.Button btn_find_export_path;
        private System.Windows.Forms.Button btn_find_table_load_path;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textboxEncryptPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBoxEncryptOption;
    }
}

