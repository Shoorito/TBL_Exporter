using System.Data;
using ExcelDataReader;
using System.IO;

namespace MWTools
{
    public class ExportUtil
    {
        //----------------------------------------------------------------------------
        public static DataSet LoadXlsxFile(string filePath, bool isUseHeader)
        {
            using (var xlsxFileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(xlsxFileStream))
                {
                    return excelDataReader.AsDataSet(new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = isUseHeader,
                        },
                    });
                }
            }
        }
    }
}

