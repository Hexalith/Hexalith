namespace Hexalith.Application.Repositories
{
    using System;

    public sealed class RepositoryMetadata : IRepositoryMetadata
    {
        public string? CausationId { get; set; }
        public string? CorrelationId { get; set; }
        public DateTime SystemUtcDateTime { get; set; }
        public DateTimeOffset UserDateTime { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string MessageId { get; set; } = string.Empty;
    }
}