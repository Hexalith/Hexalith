namespace Hexalith.Application.Repositories
{
    using System;

    public interface IRepositoryMetadata
    {
        string MessageId { get; }
        string? CausationId { get; }
        string? CorrelationId { get; }
        DateTime SystemUtcDateTime { get; }
        DateTimeOffset UserDateTime { get; }
        string UserName { get; }
    }
}