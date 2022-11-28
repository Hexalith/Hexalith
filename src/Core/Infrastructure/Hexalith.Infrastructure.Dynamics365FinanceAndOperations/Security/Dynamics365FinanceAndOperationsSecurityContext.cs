// <copyright file="Dynamics365FinanceAndOperationsSecurityContext.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Security;

using Hexalith.Infrastructure.AzureActiveDirectory;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Configurations;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class Dynamics365FinanceAndOperationsSecurityContext : AzureActiveDirectoryApplicationSecurityContext, IDynamics365FinanceAndOperationsSecurityContext
{
	private readonly string[] scopes;

	/// <summary>
	/// Initializes a new instance of the <see cref="Dynamics365FinanceAndOperationsSecurityContext" /> class.
	/// </summary>
	/// <param name="settings"></param>
	/// <param name="logger"></param>
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
			throw new ArgumentException(
				$"The {nameof(s.Instance)} setting is not defined.",
				nameof(settings));
		}

		scopes = new[] { $"{s.Instance.OriginalString}/.default" };
	}

	public Task<string> AcquireToken(CancellationToken cancellationToken)
	{
		return AcquireTokenAsync(scopes, cancellationToken);
	}
}