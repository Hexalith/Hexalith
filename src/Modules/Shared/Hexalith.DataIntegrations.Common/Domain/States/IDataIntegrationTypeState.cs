using System.Collections.Generic;

using Hexalith.DataIntegrations.Contracts.ValueTypes;

namespace Hexalith.DataIntegrations.Common.Domain.States
{
    public interface IDataIntegrationTypeState
    {
        string Description { get; set; }
        IEnumerable<DataIntegrationField> Fields { get; set; }
        string Name { get; set; }

        void Apply(IEnumerable<object> events);
    }
}