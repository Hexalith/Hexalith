namespace Hexalith.Domain.Contracts.Commands
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class CommandAttribute : Attribute
    {
        private static string GetDebuggerDisplay() => "Command";
    }
}