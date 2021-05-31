namespace Hexalith.SalesHistory.Infrastructure.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.SalesHistory.Common.Application.Services;
    using Hexalith.SalesHistory.Common.Application.States;

    public class SalesHistoryRepository : ISalesHistoryRepository
    {
        private readonly SalesHistoryDbContext _context;

        public SalesHistoryRepository(SalesHistoryDbContext context)
        {
            _context = context;
        }

        IQueryable<SalesHistoryState> ISalesHistoryRepository.Sales => _context.SalesHistory;

        public async Task AddSales(IEnumerable<SalesHistoryState> sales, CancellationToken cancellationToken = default)
        {
            _context.SalesHistory.AddRange(sales);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}