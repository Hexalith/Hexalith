namespace Hexalith.Infrastructure.Dataset
{
    using System;
    using System.Data;
    using System.IO;

    using ExcelDataReader;

    public class CsvDataSet : DynamicDataSet
    {
        private readonly string _base64Csv;

        public CsvDataSet(string base64csv, string defaultTableName = "Table") : base(defaultTableName)
        {
            _base64Csv = base64csv;
        }

        protected override DataSet InitializeDataset()
        {
            using IExcelDataReader reader = ExcelReaderFactory.CreateCsvReader(new MemoryStream(Convert.FromBase64String(_base64Csv)),
                   new ExcelReaderConfiguration
                   {
                       AutodetectSeparators = new char[] { ',', ';', '\t', '|', '#' },
                       AnalyzeInitialCsvRows = 10000
                   });
            return reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });
        }
    }
}