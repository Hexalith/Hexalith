namespace Hexalith.SalesHistory.Common.Application.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.SalesHistory.Common.Application.States;

    public interface ISalesHistoryRepository
    {
        public IQueryable<SalesHistoryState> Sales { get; }

        public Task AddSales(IEnumerable<SalesHistoryState> sales, CancellationToken cancellationToken = default);
    }
}