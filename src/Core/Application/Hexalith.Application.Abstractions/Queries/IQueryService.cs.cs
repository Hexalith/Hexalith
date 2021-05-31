namespace Hexalith.Application.Queries
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IQueryService
    {
        Task<TResult> Ask<TQuery, TResult>(TQuery query, string? messageId = null, CancellationToken cancellationToken = default) where TQuery : class;
    }
}