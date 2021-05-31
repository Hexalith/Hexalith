namespace Hexalith.WorkItems.Application.QueryHandlers
{
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

    using Fd = Infrastructure.DevOps.WorkItemFieldType;

    public class GetIssuesWithSlaHandler : QueryHandler<GetIssuesWithSla, List<IssueWithSla>>
    {
        private readonly IQueryBus _queryDispatcher;

        public GetIssuesWithSlaHandler(IQueryBus queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        public async override Task<List<IssueWithSla>> Handle(Envelope<GetIssuesWithSla> envelope, CancellationToken cancellationToken = default)
        {
            var settings = await _queryDispatcher
                .Dispatch<GetWorkItemModuleSettings, WorkItemModuleSettings>(new Envelope<GetWorkItemModuleSettings>(new GetWorkItemModuleSettings(), new MessageId(), envelope), cancellationToken).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(settings.AzureDevOpsServerUrl) || string.IsNullOrWhiteSpace(settings.PersonalAccessToken))
            {
                throw new DevOpsServerConfigurationMissingException();
            }
            if (string.IsNullOrWhiteSpace(settings.SlaGroupName))
            {
                throw new MissingSettingsException<WorkItemModuleSettings>(nameof(WorkItemModuleSettings.SlaGroupName));
            }
            var slaMembers = await _queryDispatcher
                .Dispatch<GetSecurityGroupMembers, IEnumerable<SecurityGroupMember>>(
                new Envelope<GetSecurityGroupMembers>(new GetSecurityGroupMembers(settings.SlaGroupName), new MessageId(), envelope), cancellationToken
                ).ConfigureAwait(false);

            var server = new DevOpsServer(settings.AzureDevOpsServerUrl, settings.PersonalAccessToken);

            server.Connect();
            const string? wiql = "Select [Id] " +
                    "From WorkItems " +
                    "Where [Work Item Type] = 'Issue' " +
                    "Order By [State] Asc, [Changed Date] Desc";

            var query = new Query(server, wiql, new[]
            {
                Fd.Id,
                Fd.Title,
                Fd.State,
                Fd.TeamProject,
                Fd.AssignedTo,
                Fd.Priority,
                Fd.CreatedDate,
                Fd.ClosedDate
            });
            List<DevOpsWorkItem> wis = await query.GetQueryWorkItems().ConfigureAwait(false);
            var issues = new List<IssueWithSla>(wis.Count);
            foreach (var wi in wis)
            {
                var wiHistory = await _queryDispatcher
                    .Dispatch<GetWorkItemChangeHistory, IEnumerable<WorkItemChange>>(
                    new Envelope<GetWorkItemChangeHistory>(
                        new GetWorkItemChangeHistory(
                            new WorkItemId(wi.Id.ToString(CultureInfo.InvariantCulture))), new MessageId(), envelope), cancellationToken
                    ).ConfigureAwait(false);
                var slaLog = new WorkItemSlaLog(wiHistory
                    .OrderBy(p => p.ChangeDate)
                    .Select(p => new WorkItemSlaLogItem
                    {
                        DateTime = p.ChangeDate,
                        InSla = slaMembers.Any(o => o.Name == p.UserName),
                        State = p.State
                    }));
                const int acknowledgeTime = 3600;
                const int resolutionTime = 3600 * 8 * 5;
                issues.Add(new IssueWithSla(wi.ToWorkItem(), slaLog, wi.HtmlUrl, acknowledgeTime, resolutionTime));
            }
            return issues;
        }
    }
}