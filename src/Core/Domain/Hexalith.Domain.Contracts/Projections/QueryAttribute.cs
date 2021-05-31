namespace Hexalith.Domain.Contracts.Projections
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class QueryAttribute : Attribute
    {
        private static string GetDebuggerDisplay() => "Query";
    }
}