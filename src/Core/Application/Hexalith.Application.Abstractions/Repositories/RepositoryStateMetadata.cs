namespace Hexalith.Application.Repositories
{
    using System;

    public sealed class RepositoryStateMetadata : IRepositoryStateMetadata
    {
        public string CreatedByUser { get; set; } = string.Empty;
        public DateTime CreatedUtcDateTime { get; set; }
        public string? LastModifiedByUser { get; set; } = string.Empty;
        public DateTime? LastModifiedUtcDateTime { get; set; }
    }
}