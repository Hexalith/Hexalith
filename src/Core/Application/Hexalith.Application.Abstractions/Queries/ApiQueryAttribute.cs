using System;
using System.Diagnostics;

namespace Hexalith.Application.Queries
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ApiQueryAttribute : Attribute
    {
        public ApiQueryAttribute(Type ResultType)
        {
            this.ResultType = ResultType;
        }

        public Type ResultType { get; set; }

        private static string GetDebuggerDisplay() => "ApiQuery";
    }
}