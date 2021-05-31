using System;
using System.Runtime.Serialization;

using Hexalith.WorkItems.Application.ModelViews;

namespace Hexalith.WorkItems.Application.Exceptions
{
    [Serializable]
    public class DevOpsServerConfigurationMissingException : Exception
    {
        public DevOpsServerConfigurationMissingException() : this(string.Empty)
        {
        }

        public DevOpsServerConfigurationMissingException(string? message) : this(message, null)
        {
        }

        public DevOpsServerConfigurationMissingException(string? message, Exception? innerException)
            : base($"To connect to the DevOps server, '{nameof(WorkItemModuleSettings)}:{nameof(WorkItemModuleSettings.AzureDevOpsServerUrl)}' and '{nameof(WorkItemModuleSettings)}:{nameof(WorkItemModuleSettings.PersonalAccessToken)}' settings must be defined.\n{message}", innerException)
        {
        }

        protected DevOpsServerConfigurationMissingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}