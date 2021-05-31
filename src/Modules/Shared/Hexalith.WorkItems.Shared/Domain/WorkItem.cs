namespace Hexalith.WorkItems.Domain.Entities
{
    using System;

    public class WorkItem
    {
        public string AssignedTo { get; init; } = string.Empty;

        public string ChangedBy { get; init; } = string.Empty;

        public DateTime? ChangedDate { get; init; }
        public DateTime? ClosedDate { get; init; }
        public DateTime CreatedDate { get; init; }
        public WorkItemId Id { get; init; } = new WorkItemId();
        public long Priority { get; init; }
        public string Project { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
    }
}