namespace Hexalith.Infrastructure.Dataset
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Dynamic;
    using System.Globalization;

    public class DynamicDataSet
    {
        private readonly string _defaultTableName;
        private DataSet? _dataset;
        private IDictionary<string, object?>? _value;

        public DynamicDataSet(DataSet dataset, string defaultTableName = "Table")
        {
            _dataset = dataset;
            _defaultTableName = defaultTableName;
        }

        protected DynamicDataSet(string defaultTableName = "Table")
        {
            _defaultTableName = defaultTableName;
        }

        public DataSet Dataset => _dataset ??= InitializeDataset();
        public dynamic Value => _value ??= ToDynamic();

        protected virtual DataSet InitializeDataset()
        {
            throw new NotSupportedException();
        }

        private static string Normalize(string name)
        {
            var normalized = string.Empty;
            bool ToUpper = true;
            foreach (char c in name)
            {
                if (char.IsDigit(c) || char.IsLetter(c))
                {
                    normalized += (ToUpper) ? char.ToUpper(c, CultureInfo.InvariantCulture) : c;
                    ToUpper = false;
                }
                else
                {
                    ToUpper = true;
                }
            }
            return normalized;
        }

        private string GetTableName(string tableName, int rank)
            => string.IsNullOrWhiteSpace(tableName) ? _defaultTableName + (rank + 1) : tableName;

        private dynamic ToDynamic()
        {
            var tablesCount = Dataset.Tables.Count;
            if (tablesCount == 0)
            {
                return new Dictionary<string, object?>() { { _defaultTableName, Array.Empty<object>() } };
            }
            var result = new Dictionary<string, object?>();
            var resultDict = (IDictionary<string, object?>)result;
            for (int i = 0; i < Dataset.Tables.Count; i++)
            {
                var table = Dataset.Tables[i];
                List<object> dynRows = new();
                Dictionary<string, string> columns = new();
                foreach (DataColumn column in table.Columns)
                {
                    columns.Add(column.ColumnName, Normalize(column.ColumnName));
                }
                for (int j = 0; j < table.Rows.Count; j++)
                {
                    var row = table.Rows[j];
                    var dynRow = new ExpandoObject();
                    var rowDict = (IDictionary<string, object?>)dynRow;
                    foreach (var column in columns)
                    {
                        var value = row[column.Key];
                        rowDict[column.Value] = Convert.IsDBNull(value) ? null : value;
                    }
                    dynRows.Add(dynRow);
                }
                resultDict.Add(Dataset.Tables.Count == 1 ? _defaultTableName : GetTableName(table.TableName, i), dynRows);
            }
            return result;
        }
    }
}