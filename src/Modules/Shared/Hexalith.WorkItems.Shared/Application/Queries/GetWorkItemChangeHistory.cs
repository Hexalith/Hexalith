namespace Hexalith.WorkItems.Application.Queries
{
    using Hexalith.WorkItems.Domain;

    public class GetWorkItemChangeHistory
    {
        public GetWorkItemChangeHistory(WorkItemId workItemId)
        {
            WorkItemId = workItemId;
        }

        public string WorkItemId { get; }
    }
}