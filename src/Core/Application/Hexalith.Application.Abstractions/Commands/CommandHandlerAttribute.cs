using System;
using System.Diagnostics;

namespace Hexalith.Application.Commands
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CommandHandlerAttribute : Attribute
    {
        public Type? Command { get; set; }

        private string DebuggerDisplay => $"{Command?.Name} command handler";
    }
}