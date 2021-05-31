namespace Hexalith.DataIntegrations.Common.Domain.States
{
    using System;
    using System.Collections.Generic;

    using Hexalith.DataIntegrations.Contracts.Events;
    using Hexalith.DataIntegrations.Contracts.ValueTypes;

    public class DataIntegrationTypeState : IDataIntegrationTypeState
    {
        public string Description { get; set; } = string.Empty;
        public IEnumerable<DataIntegrationField> Fields { get; set; } = Array.Empty<DataIntegrationField>();
        public string Name { get; set; } = string.Empty;

        public void Apply(IEnumerable<object> events)
        {
            foreach (var @event in events)
            {
                switch (@event)
                {
                    case NewDataIntegrationTypeDefined submitted:
                        Apply(submitted);
                        break;

                    default:
                        throw new NotSupportedException($"Event type '{@event.GetType().Name} is not supported by '{nameof(DataIntegrationTypeState)}''");
                }
            }
        }

        private void Apply(NewDataIntegrationTypeDefined newDefined)
        {
            Name = newDefined.Name;
            Description = newDefined.Description;
            Fields = newDefined.Fields;
        }
    }
}