namespace Hexalith.WorkItems.Application.QueryHandlers
{
    using Hexalith.Application.Queries;
    using Hexalith.WorkItems.Application.ModelViews;
    using Hexalith.WorkItems.Application.Queries;

    using Microsoft.Extensions.Options;

    public sealed class GetWorkItemModuleSettingsHandler : SettingsQueryHandler<GetWorkItemModuleSettings, WorkItemModuleSettings>
    {
        public GetWorkItemModuleSettingsHandler(IOptions<WorkItemModuleSettings> options) : base(options)
        {
        }
    }
}