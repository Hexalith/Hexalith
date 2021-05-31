namespace Hexalith.WorkItems.Application.Queries
{
    using System;
    using System.Collections.Generic;

    using Hexalith.Application.Queries;
    using Hexalith.WorkItems.Application.ModelViews;

    public sealed class GetIssuesWithSla : QueryBase<List<IssueWithSla>>
    {
        public GetIssuesWithSla(bool suspendedSla, bool closedIssues)
        {
            SuspendedSla = suspendedSla;
            ClosedIssues = closedIssues;
        }

        [Obsolete("For serialization only")]
        public GetIssuesWithSla()
        {
        }

        public bool ClosedIssues { get; }
        public bool SuspendedSla { get; }
    }
}