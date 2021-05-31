namespace Hexalith.Application.Repositories
{
    using System.Collections.Generic;

    public interface IRepositoryData<out TState> : IRepositoryData
    {
        new TState State { get; }
    }

    public interface IRepositoryData
    {
        IEnumerable<object> Events { get; }
        IRepositoryMetadata Metadata { get; }
        object State { get; }
    }
}