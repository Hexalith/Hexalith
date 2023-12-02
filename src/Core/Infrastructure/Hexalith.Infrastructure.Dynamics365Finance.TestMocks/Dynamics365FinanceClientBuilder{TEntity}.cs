// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.TestMocks
// Author           : Jérôme Piquot
// Created          : 10-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-03-2023
// ***********************************************************************
// <copyright file="Dynamics365FinanceClientBuilder{TEntity}.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.TestMocks;

using Hexalith.Infrastructure.Dynamics365Finance.Client;
using Hexalith.Infrastructure.Dynamics365Finance.Configurations;
using Hexalith.Infrastructure.Dynamics365Finance.Models;
using Hexalith.TestMocks;

using Moq;

/// <summary>
/// Dynamics 365 for finance and operations mock build.
/// </summary>
/// <typeparam name="TEntity">The type of the Dynamics 365 entity.</typeparam>
public class Dynamics365FinanceClientBuilder<TEntity> :
    IMockBuilder<IDynamics365FinanceClient<TEntity>>
    where TEntity : class, IODataElement
{
    /// <summary>
    /// The HTTP clientfactory.
    /// </summary>
    private HttpClientFactoryBuilder? _httpClientfactory;

    /// <summary>
    /// The logger.
    /// </summary>
    private LoggerBuilder<Dynamics365FinanceClient<TEntity>>? _logger;

    /// <summary>
    /// The security context.
    /// </summary>
    private Dynamics365FinanceSecurityContextBuilder? _securityContext;

    /// <summary>
    /// The settings.
    /// </summary>
    private OptionsBuilder<Dynamics365FinanceClientSettings>? _settings;

    /// <summary>
    /// Gets the <see cref="IHttpClientFactory" /> builder configuration.
    /// </summary>
    /// <value>The HTTP clientfactory.</value>
    public HttpClientFactoryBuilder HttpClientfactory => _httpClientfactory ??= new HttpClientFactoryBuilder();

    /// <summary>
    /// Gets the <see cref="ILogger{Dynamics365FinanceClient}" /> builder configuration.
    /// </summary>
    /// <value>The logger.</value>
    public LoggerBuilder<Dynamics365FinanceClient<TEntity>> Logger
            => _logger ??= new LoggerBuilder<Dynamics365FinanceClient<TEntity>>();

    /// <summary>
    /// Gets the <see cref="IDynamics365FinanceSecurityContext" /> builder configuration.
    /// </summary>
    /// <value>The security context.</value>
    public Dynamics365FinanceSecurityContextBuilder SecurityContext
        => _securityContext ??= new Dynamics365FinanceSecurityContextBuilder();

    /// <summary>
    /// Gets the <see cref="IOptions{Dynamics365FinanceSettings}" /> builder configuration.
    /// </summary>
    /// <value>The settings.</value>
    public OptionsBuilder<Dynamics365FinanceClientSettings> Settings
                => _settings ??= new OptionsBuilder<Dynamics365FinanceClientSettings>();

    /// <inheritdoc/>
    public IDynamics365FinanceClient<TEntity> Build()
    {
        return _settings is null || !_settings.HasValue
            ? BuildMock().Object
            : new Dynamics365FinanceClient<TEntity>(
                HttpClientfactory.Build(),
                SecurityContext.Build(),
                Settings.Build(),
                Logger.Build());
    }

    /// <inheritdoc/>
    public IMock<IDynamics365FinanceClient<TEntity>> BuildMock()
    {
        Mock<IDynamics365FinanceClient<TEntity>> client = new();
        return client;
    }

    /// <summary>
    /// Sets the settings value used for the mocked client.
    /// </summary>
    /// <param name="settings">The settings value.</param>
    /// <returns>The client builder.</returns>
    public Dynamics365FinanceClientBuilder<TEntity> WithSettingsValue(Dynamics365FinanceClientSettings settings)
    {
        _ = Settings.WithValue(settings);
        _ = SecurityContext.Settings.WithValue(settings);
        return this;
    }

    /// <summary>
    /// Sets the settings from the appsettings.json file and the .NET user secrets.
    /// </summary>
    /// <typeparam name="TProgram">The test class object to define the assembly used for .NET user secrets.</typeparam>
    /// <returns>The client builder.</returns>
    public Dynamics365FinanceClientBuilder<TEntity> WithValueFromConfiguration<TProgram>()
        where TProgram : class
    {
        _ = Settings.WithValueFromConfiguration<TProgram>();
        _ = SecurityContext.Settings.WithValueFromConfiguration<TProgram>();
        return this;
    }
}