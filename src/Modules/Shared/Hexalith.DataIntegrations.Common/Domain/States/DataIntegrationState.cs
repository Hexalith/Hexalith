namespace Hexalith.DataIntegrations.Domain.States
{
    using System;
    using System.Collections.Generic;

    using Hexalith.DataIntegrations.Contracts.Events;

    public sealed class DataIntegrationState : IDataIntegrationState
    {
        public dynamic? Data { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Document { get; set; } = string.Empty;
        public string DocumentName { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public void Apply(IEnumerable<object> events)
        {
            foreach (var @event in events)
            {
                switch (@event)
                {
                    case DataIntegrationSubmitted submitted:
                        Apply(submitted);
                        break;

                    case DataIntegrationNormalized normalized:
                        Apply(normalized);
                        break;

                    default:
                        throw new NotSupportedException($"Event type '{@event.GetType().Name} is not supported by '{nameof(DataIntegrationState)}''");
                }
            }
        }

        private void Apply(DataIntegrationSubmitted submitted)
        {
            Name = submitted.Name;
            Description = submitted.Description;
            DocumentName = submitted.DocumentName;
            DocumentType = submitted.DocumentType;
            Document = submitted.Document;
        }

        private void Apply(DataIntegrationNormalized normalized)
        {
            Data = normalized.Data;
        }
    }
}