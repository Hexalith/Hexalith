// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.TestMocks
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-13-2023
// ***********************************************************************
// <copyright file="Dynamics365FinanceSecurityContextBuilder.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.TestMocks;

using Hexalith.Infrastructure.Dynamics365Finance.Configurations;
using Hexalith.Infrastructure.Dynamics365Finance.Security;
using Hexalith.TestMocks;

using Moq;

/// <summary>
/// Test mock builder for <see cref="IDynamics365FinanceSecurityContext" />.
/// </summary>
public class Dynamics365FinanceSecurityContextBuilder
{
    /// <summary>
    /// The logger.
    /// </summary>
    private LoggerBuilder<Dynamics365FinanceSecurityContext>? _logger;

    /// <summary>
    /// The settings.
    /// </summary>
    private OptionsBuilder<Dynamics365FinanceClientSettings>? _settings;

    /// <summary>
    /// Gets logger to configure.
    /// </summary>
    /// <value>The logger.</value>
    public LoggerBuilder<Dynamics365FinanceSecurityContext> Logger
        => _logger ??= new LoggerBuilder<Dynamics365FinanceSecurityContext>();

    /// <summary>
    /// Gets the settings to configure.
    /// </summary>
    /// <value>The settings.</value>
    public OptionsBuilder<Dynamics365FinanceClientSettings> Settings
            => _settings ??= new OptionsBuilder<Dynamics365FinanceClientSettings>();

    /// <summary>
    /// Build a Mock object.
    /// </summary>
    /// <returns>Security context mock.</returns>
    public static Mock<IDynamics365FinanceSecurityContext> BuildMock()
    {
        Mock<IDynamics365FinanceSecurityContext> security = new();
        _ = security
            .Setup(x => x.AcquireTokenAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync("token");
        return security;
    }

    /// <summary>
    /// Build the security context with mocked injected dependencies.
    /// </summary>
    /// <returns>Security context mock.</returns>
    public IDynamics365FinanceSecurityContext Build()
    {
        if (_settings is null || !_settings.HasValue)
        {
            return BuildMock().Object;
        }

        Microsoft.Extensions.Options.IOptions<Dynamics365FinanceClientSettings> settings = Settings.Build();
        return (settings.Value.Identity == null)
            ? BuildMock().Object
            : new Dynamics365FinanceSecurityContext(
            Settings.Build(),
            Logger.Build());
    }
}