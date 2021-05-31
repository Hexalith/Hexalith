namespace Hexalith.DataIntegrations.Common.Domain.States
{
    using System;
    using System.Collections.Generic;

    using Hexalith.DataIntegrations.Contracts.ValueTypes;

    public class DataIntegrationSourceState
    {
        public IEnumerable<string> ContentFilters { get; set; } = Array.Empty<string>();
        public string Description { get; set; } = string.Empty;
        public string DescriptionFilter { get; set; } = string.Empty;
        public IEnumerable<DataIntegrationFieldMap> FieldMap { get; set; } = Array.Empty<DataIntegrationFieldMap>();
        public IEnumerable<DataIntegrationField> Fields { get; set; } = Array.Empty<DataIntegrationField>();
        public string FileNameFilter { get; set; } = string.Empty;
        public string FileTypeFilter { get; set; } = string.Empty;
        public string IntegrationType { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string NameFilter { get; set; } = string.Empty;
        public int Priority { get; set; }
    }
}