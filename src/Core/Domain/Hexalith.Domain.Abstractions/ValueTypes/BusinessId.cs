namespace Hexalith.Domain.ValueTypes
{
    using System.Diagnostics;

    [DebuggerDisplay("{Value}")]
    public abstract class BusinessId : AutoIdentifier
    {
        protected BusinessId(string value)
            : base(value)
        {
        }

        protected BusinessId(BusinessId value)
            : base(value)
        {
        }

        protected BusinessId()
        {
        }
    }
}