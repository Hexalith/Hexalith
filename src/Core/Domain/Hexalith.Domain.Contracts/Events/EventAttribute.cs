namespace Hexalith.Domain.Contracts.Events
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class EventAttribute : Attribute
    {
        private static string GetDebuggerDisplay() => "Event";
    }
}