using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Hexalith.WorkItems.Infrastructure.DevOps
{
    [Serializable]
    public class DevOpsGroupNotFoundException : Exception
    {
        public DevOpsGroupNotFoundException() : this(null)
        {
        }

        public DevOpsGroupNotFoundException(string? message) : this(null, null, message)
        {
        }

        public DevOpsGroupNotFoundException(string? groupName, IEnumerable<string>? availableGroups, string? message = null, Exception? innerException = null)
            : base($"Could not find DevOps group '{groupName}'.{message}", innerException)
        {
            AvailableGroups = availableGroups?.OrderBy(p => p);
            GroupName = groupName;
        }

        public DevOpsGroupNotFoundException(string? message, Exception? innerException) : this(null, null, message, innerException)
        {
        }

        protected DevOpsGroupNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public IEnumerable<string>? AvailableGroups { get; }
        public string? GroupName { get; }
    }
}