namespace Hexalith.Application.Repositories
{
    using System;

    public interface IRepositoryStateMetadata
    {
        string CreatedByUser { get; set; }
        DateTime CreatedUtcDateTime { get; set; }
        string? LastModifiedByUser { get; set; }
        DateTime? LastModifiedUtcDateTime { get; set; }
    }
}