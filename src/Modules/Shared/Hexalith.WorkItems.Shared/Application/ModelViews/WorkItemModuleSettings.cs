using System.Collections.Generic;

using Hexalith.WorkItems.Application.Models;

namespace Hexalith.WorkItems.Application.ModelViews
{
    public sealed class WorkItemModuleSettings
    {
        public string? AzureDevOpsServerUrl { get; init; }
        public string? ClientId { get; init; }
        public string? ClientSecret { get; init; }
        public string? PersonalAccessToken { get; init; }
        public List<ProjectSla> ProjectSlas { get; init; } = new List<ProjectSla>();
        public string? SlaGroupName { get; init; }
    }
}