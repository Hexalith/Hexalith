namespace Hexalith.WorkItems.Application.ModelViews
{
    using System;

    using Hexalith.WorkItems.Domain;
    using Hexalith.WorkItems.Domain.Entities;

    public sealed class IssueWithSla
    {
        public IssueWithSla()
        {
        }

        public IssueWithSla(WorkItem wi, WorkItemSlaLog slaLog, string workItemUrl, int slaAcknowledgmentTime, int slaResolutionTime)
        {
            WorkItemId = wi.Id;
            WorkItemUrl = workItemUrl;
            Title = wi.Title;
            ProjectName = wi.Project;
            Assignee = wi.AssignedTo;
            Priority = (int)wi.Priority;
            CreatedDateTime = wi.CreatedDate;
            ResolvedDateTime = slaLog.ResolvedDateTime;
            AcknowledgmentDateTime = slaLog.AcknowledgedDateTime;
            AcknowledgmentTimeInSeconds = slaLog.AcknowledgmentTimeInSeconds;
            AcknowledgmentRemainingTimeInSeconds = slaLog.GetAcknowledgmentRemainingSeconds(slaAcknowledgmentTime);
            ClosedDateTime = wi.ClosedDate;
            ResolutionTimeInSeconds = slaLog.ResolutionTimeInSeconds;
            RemainingResolutionTimeInSeconds = slaLog.GetResolutionRemainingSeconds(slaResolutionTime);
            SlaSuspendedTimeInSeconds = slaLog.SlaSuspendedTimeInSeconds;
            SlaActiveTimeInSeconds = slaLog.SlaActiveTimeInSeconds;
        }

        public DateTime? AcknowledgmentDateTime { get; init; }
        public int AcknowledgmentTimeInSeconds { get; }
        public int AcknowledgmentRemainingTimeInSeconds { get; init; }
        public string? Assignee { get; init; }
        public DateTime? ClosedDateTime { get; init; }
        public DateTime? ResolvedDateTime { get; init; }
        public DateTime CreatedDateTime { get; init; }
        public int Priority { get; init; }
        public string? ProjectName { get; init; }
        public int RemainingResolutionTimeInSeconds { get; init; }
        public int ResolutionTimeInSeconds { get; init; }
        public int SlaActiveTimeInSeconds { get; init; }
        public int SlaSuspendedTimeInSeconds { get; init; }
        public string? Title { get; init; }
        public string WorkItemId { get; init; } = string.Empty;
        public string? WorkItemUrl { get; init; }
    }
}