namespace Hexalith.Domain.ValueTypes
{
    using System.Diagnostics;

    [DebuggerDisplay("{Value}")]
    public abstract class StringValue : SingleValueType<string>
    {
        protected StringValue()
        {
        }

        protected StringValue(StringValue value)
            : base(value)
        {
        }

        protected StringValue(string value)
            : base(value)
        {
        }
    }
}