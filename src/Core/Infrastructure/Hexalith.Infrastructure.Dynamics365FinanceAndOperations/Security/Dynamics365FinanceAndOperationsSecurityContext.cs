// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Security;

using Hexalith.Infrastructure.AzureActiveDirectory;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Configurations;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System.Threading;
using System.Threading.Tasks;

public class Dynamics365FinanceAndOperationsSecurityContext : AzureActiveDirectoryApplicationSecurityContext, IDynamics365FinanceAndOperationsSecurityContext
{
	private readonly string[] _scopes;

	public Dynamics365FinanceAndOperationsSecurityContext(
		IOptions<Dynamics365FinanceAndOperationsClientSettings> settings,
		ILogger<AzureActiveDirectoryApplicationSecurityContext> logger)
		: base(
			settings.Value?.Identity ?? throw new ArgumentNullException(nameof(settings)),
			logger)
	{
		Dynamics365FinanceAndOperationsClientSettings s = settings.Value ?? throw new ArgumentNullException(nameof(settings));
		if (string.IsNullOrWhiteSpace(s.Instance?.OriginalString))
		{
			throw new ArgumentException($"The {nameof(s.Instance)} setting is not defined.",
										nameof(settings));
		}
		_scopes = new[] { $"{s.Instance.OriginalString}/.default" };
	}

	public async Task<string> AcquireToken(CancellationToken cancellationToken)
		=> await AcquireToken(_scopes, cancellationToken);
}