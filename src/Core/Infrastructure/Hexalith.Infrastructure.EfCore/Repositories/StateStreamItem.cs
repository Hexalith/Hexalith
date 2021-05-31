using System;

namespace Hexalith.Infrastructure.EfCore.Repositories
{
    [Serializable]
    public class StateStreamItem
    {
        public string? CausationId { get; set; }
        public string? CorrelationId { get; set; }
        public string Events { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public int IdHash { get; set; }
        public string MessageId { get; set; } = string.Empty;
        public DateTime SystemUtcDateTime { get; set; }
        public DateTimeOffset UserDateTime { get; }
        public string UserName { get; set; } = string.Empty;
        public int Version { get; set; }
    }
}