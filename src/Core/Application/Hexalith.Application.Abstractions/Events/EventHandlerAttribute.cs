using System;
using System.Diagnostics;

namespace Hexalith.Application.Events
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class EventHandlerAttribute : Attribute
    {
        public Type? Event { get; set; }

        private string DebuggerDisplay => $"{Event?.Name} event handler";
    }
}