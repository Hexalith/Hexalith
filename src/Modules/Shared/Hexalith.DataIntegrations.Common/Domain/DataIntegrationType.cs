using System.Collections.Generic;
using System.Threading.Tasks;

using Hexalith.DataIntegrations.Common.Domain.States;
using Hexalith.DataIntegrations.Contracts.Events;
using Hexalith.DataIntegrations.Contracts.ValueTypes;

namespace Hexalith.DataIntegrations.Domain
{
    internal class DataIntegrationType
    {
        private readonly string _id;
        private readonly IDataIntegrationTypeState _state;

        public DataIntegrationType(string id, IDataIntegrationTypeState state)
        {
            _id = id;
            _state = state;
        }

        public Task<IEnumerable<object>> DefineNew(
            string name,
            string description,
            IEnumerable<DataIntegrationField> fields)
        {
            List<object> events = new();
            events.Add(new NewDataIntegrationTypeDefined
            {
                DataIntegrationTypeId = _id,
                Name = name,
                Description = description,
                Fields = fields
            });
            _state.Apply(events);
            return Task.FromResult<IEnumerable<object>>(events);
        }
    }
}