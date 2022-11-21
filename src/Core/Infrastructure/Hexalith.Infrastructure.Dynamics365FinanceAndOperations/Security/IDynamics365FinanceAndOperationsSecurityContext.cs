// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Security;

using Hexalith.Infrastructure.Security.Abstractions;

using System.Threading.Tasks;

public interface IDynamics365FinanceAndOperationsSecurityContext : IApplicationSecurityContext
{
	Task<string> AcquireToken(CancellationToken cancellationToken = default);
}