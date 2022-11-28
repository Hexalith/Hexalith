// <copyright file="Dynamics365FinanceAndOperationsSecurityContextBuilder.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.TestMocks;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Configurations;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Security;
using Hexalith.TestMocks;

using Moq;

/// <summary>
/// Test mock builder for <see cref="IDynamics365FinanceAndOperationsSecurityContext" />.
/// </summary>
public class Dynamics365FinanceAndOperationsSecurityContextBuilder
{
	private LoggerBuilder<Dynamics365FinanceAndOperationsSecurityContext>? _logger = null;
	private OptionsBuilder<Dynamics365FinanceAndOperationsClientSettings>? _settings = null;

	/// <summary>
	/// Gets logger to configure.
	/// </summary>
	public LoggerBuilder<Dynamics365FinanceAndOperationsSecurityContext> Logger
		=> _logger ??= new LoggerBuilder<Dynamics365FinanceAndOperationsSecurityContext>();

	/// <summary>
	/// Gets the settings to configure.
	/// </summary>
	public OptionsBuilder<Dynamics365FinanceAndOperationsClientSettings> Settings
			=> _settings ??= new OptionsBuilder<Dynamics365FinanceAndOperationsClientSettings>();

	/// <summary>
	/// Build a Mock object.
	/// </summary>
	/// <returns>Security context mock.</returns>
	public static Mock<IDynamics365FinanceAndOperationsSecurityContext> BuildMock()
	{
		Mock<IDynamics365FinanceAndOperationsSecurityContext> security = new();
		_ = security
			.Setup(x => x.AcquireTokenAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync("token");
		return security;
	}

	/// <summary>
	/// Build the security context with mocked injected dependencies.
	/// </summary>
	/// <returns>Security context mock.</returns>
	public IDynamics365FinanceAndOperationsSecurityContext Build()
	{
		if (_settings is null || !_settings.HasValue)
		{
			return BuildMock().Object;
		}

		Microsoft.Extensions.Options.IOptions<Dynamics365FinanceAndOperationsClientSettings> settings = Settings.Build();
		return (settings.Value.Identity == null)
			? BuildMock().Object
			: new Dynamics365FinanceAndOperationsSecurityContext(
			Settings.Build(),
			Logger.Build());
	}
}