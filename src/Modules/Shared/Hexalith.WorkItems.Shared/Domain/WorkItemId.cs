namespace Hexalith.WorkItems.Domain
{
    using Hexalith.Domain.ValueTypes;

    public class WorkItemId : BusinessId
    {
        public WorkItemId()
        {
        }

        public WorkItemId(string value) : base(value)
        {
        }

        public WorkItemId(BusinessId value) : base(value)
        {
        }
    }
}