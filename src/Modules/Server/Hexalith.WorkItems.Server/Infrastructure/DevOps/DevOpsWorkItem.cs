namespace Hexalith.WorkItems.Infrastructure.DevOps
{
    using System;
    using System.Globalization;
    using System.Linq;

    using Hexalith.WorkItems.Domain.Entities;

    using Microsoft.VisualStudio.Services.WebApi;

    using Fd = WorkItemFieldType;
    using Wi = Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem;

    internal class DevOpsWorkItem
    {
        private readonly Wi _wi;

        public DevOpsWorkItem(Wi workItem)
        {
            _wi = workItem;
        }

        public string AssignedTo
        {
            get
            {
                var user = GetField<IdentityRef?>(Fd.AssignedTo);
                if (user != null)
                {
                    return user.UniqueName;
                }
                return string.Empty;
            }
        }

        public string ChangedBy
        {
            get
            {
                var user = GetField<IdentityRef?>(Fd.ChangedBy);
                if (user != null)
                {
                    return user.UniqueName;
                }
                return string.Empty;
            }
        }

        public string HtmlUrl
        {
            get
            {
                var link = (ReferenceLink?)_wi
                    .Links
                    .Links
                    .Where(p => p.Key == "html")
                    .Select(p => p.Value)
                    .FirstOrDefault();
                return link?.Href ?? string.Empty;
            }
        }

        public DateTime? ChangedDate => GetField<DateTime?>(Fd.ChangedDate);
        public DateTime? ClosedDate => GetField<DateTime?>(Fd.ClosedDate);
        public DateTime CreatedDate => GetField<DateTime?>(Fd.CreatedDate) ?? DateTime.MinValue;
        public int Id => _wi.Id ?? 0;
        public long Priority => GetField<long>(Fd.Priority);
        public string State => GetField(Fd.State);
        public string TeamProject => GetField(Fd.TeamProject);
        public string Title => GetField(Fd.Title);

        public WorkItem ToWorkItem() => new()
        {
            AssignedTo = AssignedTo,
            ChangedBy = ChangedBy,
            ChangedDate = ChangedDate,
            ClosedDate = ClosedDate,
            CreatedDate = CreatedDate,
            Id = new Domain.WorkItemId(Id.ToString(CultureInfo.InvariantCulture)),
            Priority = Priority,
            Project = TeamProject,
            Title = Title
        };

        private string GetField(string name)
        {
            return GetField<string>(name) ?? string.Empty;
        }

        private T? GetField<T>(string name)
        {
            if (_wi.Fields.TryGetValue(name, out object? value))
            {
                return (T)value;
            }
            return default;
        }
    }
}