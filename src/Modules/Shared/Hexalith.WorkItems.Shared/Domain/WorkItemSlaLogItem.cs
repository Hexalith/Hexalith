namespace Hexalith.WorkItems.Domain
{
    using System;

    public class WorkItemSlaLogItem
    {
        public DateTime DateTime { get; init; }
        public bool InSla { get; init; }

        public WorkItemState State { get; init; }
    }
}