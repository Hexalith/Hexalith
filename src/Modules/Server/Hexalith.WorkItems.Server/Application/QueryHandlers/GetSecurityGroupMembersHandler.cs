namespace Hexalith.WorkItems.Application.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.Application.Messages;
    using Hexalith.Application.Queries;
    using Hexalith.Domain.ValueTypes;
    using Hexalith.WorkItems.Application.Exceptions;
    using Hexalith.WorkItems.Application.ModelViews;
    using Hexalith.WorkItems.Application.Queries;
    using Hexalith.WorkItems.Infrastructure.DevOps;

    public class GetSecurityGroupMembersHandler : QueryHandler<GetSecurityGroupMembers, IEnumerable<SecurityGroupMember>>
    {
        private readonly IQueryBus _queryDispatcher;

        public GetSecurityGroupMembersHandler(IQueryBus queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        public override async Task<IEnumerable<SecurityGroupMember>> Handle(Envelope<GetSecurityGroupMembers> envelope, CancellationToken cancellationToken = default)
        {
            var settings = await _queryDispatcher
                 .Dispatch<GetWorkItemModuleSettings, WorkItemModuleSettings>(new Envelope<GetWorkItemModuleSettings>(new GetWorkItemModuleSettings(), new MessageId(), envelope), cancellationToken
                 );
            if (string.IsNullOrWhiteSpace(settings.AzureDevOpsServerUrl) || string.IsNullOrWhiteSpace(settings.PersonalAccessToken))
            {
                throw new DevOpsServerConfigurationMissingException();
            }
            if (string.IsNullOrWhiteSpace(settings.SlaGroupName))
            {
                throw new MissingSettingsException<WorkItemModuleSettings>(nameof(WorkItemModuleSettings.SlaGroupName));
            }
            var server = new DevOpsServer(settings.AzureDevOpsServerUrl, settings.PersonalAccessToken);

            server.Connect();
            var devOpsGroup = new DevOpsGroup(server, settings.SlaGroupName);
            return (await devOpsGroup.GetMembers(cancellationToken))
                .Select(p => new SecurityGroupMember
                {
                    Name = p.PrincipalName ?? string.Empty
                });
        }
    }
}