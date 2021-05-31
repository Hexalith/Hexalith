namespace Hexalith.WorkItems.Application.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.Application.Messages;
    using Hexalith.Application.Queries;
    using Hexalith.Domain.ValueTypes;
    using Hexalith.WorkItems.Application.Exceptions;
    using Hexalith.WorkItems.Application.ModelViews;
    using Hexalith.WorkItems.Application.Queries;
    using Hexalith.WorkItems.Domain;
    using Hexalith.WorkItems.Infrastructure.DevOps;

    public class GetWorkItemChangeHistoryHandler : QueryHandler<GetWorkItemChangeHistory, IEnumerable<WorkItemChange>>
    {
        private readonly IQueryBus _queryDispatcher;

        public GetWorkItemChangeHistoryHandler(IQueryBus queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        public override async Task<IEnumerable<WorkItemChange>> Handle(Envelope<GetWorkItemChangeHistory> envelope, CancellationToken cancellationToken = default)
        {
            var settings = await _queryDispatcher
                .Dispatch<GetWorkItemModuleSettings, WorkItemModuleSettings>(
                    new Envelope<GetWorkItemModuleSettings>(new GetWorkItemModuleSettings(), new MessageId(), envelope), cancellationToken
                    );
            if (string.IsNullOrWhiteSpace(settings.AzureDevOpsServerUrl) || string.IsNullOrWhiteSpace(settings.PersonalAccessToken))
            {
                throw new DevOpsServerConfigurationMissingException();
            }
            var server = new DevOpsServer(settings.AzureDevOpsServerUrl, settings.PersonalAccessToken);

            server.Connect();

            var wiCollection = new WorkItemCollection(server);
            int id = Convert.ToInt32(envelope.Message.WorkItemId, CultureInfo.InvariantCulture);
            return (await wiCollection.GetWorkItemHistory(id, cancellationToken))
                .Select(p => new WorkItemChange
                {
                    ChangeDate = p.ChangedDate ?? DateTime.MinValue,
                    ChangeType = ChangeType.Update,
                    UserName = p.ChangedBy,
                    State = p.State switch
                    {
                        "New" => WorkItemState.New,
                        "Active" => WorkItemState.Active,
                        "Closed" => WorkItemState.Closed,
                        "Resolved" => WorkItemState.Resolved,
                        _ => WorkItemState.Active
                    }
                });
        }
    }
}