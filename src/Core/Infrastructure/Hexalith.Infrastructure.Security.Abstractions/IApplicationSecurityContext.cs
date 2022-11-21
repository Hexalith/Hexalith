// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Infrastructure.Security.Abstractions;

public interface IApplicationSecurityContext
{
	Task<string> AcquireToken(string[] scopes, CancellationToken cancellationToken = default);
}