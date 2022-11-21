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
	private readonly Dynamics365FinanceAndOperationsClientSettings _settings;

	public Dynamics365FinanceAndOperationsSecurityContext(
		IOptions<Dynamics365FinanceAndOperationsClientSettings> settings,
		ILogger<AzureActiveDirectoryApplicationSecurityContext> logger)
		: base(
			settings.Value?.Identity ?? throw new ArgumentNullException(nameof(settings)),
			logger)
	{
		_settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
		if (string.IsNullOrWhiteSpace(_settings.Instance?.OriginalString))
		{
			throw new ArgumentException($"The {nameof(_settings.Instance)} setting is not defined.",
										nameof(settings));
		}
		_scopes = new string[] { $"{_settings.Instance.OriginalString}/.default" };
	}

	public async Task<string> AcquireToken(CancellationToken cancellationToken = default)
		=> await AcquireToken(_scopes, cancellationToken);
}