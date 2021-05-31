namespace Hexalith.DataIntegrations.Domain
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Hexalith.DataIntegrations.Common.Domain.ValueTypes;
    using Hexalith.DataIntegrations.Contracts.Events;
    using Hexalith.DataIntegrations.Domain.States;
    using Hexalith.Infrastructure.Dataset;

    using ExcelDataReader;

    internal class DataIntegration
    {
        private readonly string _id;
        private readonly IDataIntegrationState _state;

        public DataIntegration(string id, IDataIntegrationState state)
        {
            _id = id;
            _state = state;
        }

        public Task<IEnumerable<object>> Normalize()
        {
            FileType.TryParse(typeof(FileType), _state.DocumentType, true, out object? fileType);
            dynamic data;
            var content = Convert.FromBase64String(_state.Document);
            switch (fileType)
            {
                case FileType.Xml:
                    data = _state.Document;
                    break;

                case FileType.Csv:
                    {
                        using IExcelDataReader reader = ExcelReaderFactory.CreateCsvReader(new MemoryStream(content),
                            new ExcelReaderConfiguration
                            {
                                AutodetectSeparators = new char[] { ',', ';', '\t', '|', '#' },
                                AnalyzeInitialCsvRows = 10000
                            });
                        var dataSet = new DynamicDataSet(reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                            }
                        }));
                        data = dataSet.Value;
                        break;
                    }
                case FileType.Xls:
                case FileType.Xlsx:
                case FileType.Xlsb:
                    {
                        using IExcelDataReader reader = ExcelReaderFactory.CreateReader(new MemoryStream(content));
                        var dataSet = new DynamicDataSet(reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                            }
                        }));
                        data = dataSet.Value;
                        break;
                    }
                default:
                    return Task.FromResult<IEnumerable<object>>(Array.Empty<object>());
            }
            List<object> events = new()
            {
                new DataIntegrationNormalized
                {
                    DataIntegrationId = _id,
                    Name = _state.Name,
                    Description = _state.Description,
                    Data = data,
                }
            };
            _state.Apply(events);
            return Task.FromResult<IEnumerable<object>>(events);
        }

        public Task<IEnumerable<object>> Submit(
            string name,
            string description,
            string documentName,
            string documentType,
            string document)
        {
            if (string.IsNullOrWhiteSpace(documentType))
            {
                documentType = Path.GetExtension(documentName) switch
                {
                    ".CSV" => nameof(FileType.Csv),
                    ".TXT" => nameof(FileType.Csv),
                    ".XML" => nameof(FileType.Xml),
                    ".XLS" => nameof(FileType.Xls),
                    ".XLSX" => nameof(FileType.Xlsx),
                    ".XLSB" => nameof(FileType.Xlsb),
                    _ => string.Empty
                };
            }
            List<object> events = new()
            {
                new DataIntegrationSubmitted
                {
                    DataIntegrationId = _id,
                    Name = name,
                    Description = description,
                    DocumentName = documentName,
                    DocumentType = documentType,
                    Document = document
                }
            };
            _state.Apply(events);
            return Task.FromResult<IEnumerable<object>>(events);
        }
    }
}