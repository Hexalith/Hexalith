// <copyright file="Dynamics365FinanceAndOperationsClientBuilder.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.TestMocks;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Configurations;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Models;
using Hexalith.TestMocks;

using Moq;

/// <summary>
/// Dynamics 365 for finance and operations mock build.
/// </summary>
/// <typeparam name="TEntity">The type of the Dynamics 365 entity.</typeparam>
public class Dynamics365FinanceAndOperationsClientBuilder<TEntity> :
    IMockBuilder<IDynamics365FinanceAndOperationsClient<TEntity>>
    where TEntity : IODataElement
{
    private HttpClientFactoryBuilder? _httpClientfactory;
    private LoggerBuilder<Dynamics365FinanceAndOperationsClient<TEntity>>? _logger;
    private Dynamics365FinanceAndOperationsSecurityContextBuilder? _securityContext;
    private OptionsBuilder<Dynamics365FinanceAndOperationsClientSettings>? _settings;

    /// <summary>
    /// Gets the <see cref="IHttpClientFactory"/> builder configuration.
    /// </summary>
    public HttpClientFactoryBuilder HttpClientfactory => _httpClientfactory ??= new HttpClientFactoryBuilder();

    /// <summary>
    /// Gets the <see cref="ILogger{Dynamics365FinanceAndOperationsClient}"/> builder configuration.
    /// </summary>
    public LoggerBuilder<Dynamics365FinanceAndOperationsClient<TEntity>> Logger
            => _logger ??= new LoggerBuilder<Dynamics365FinanceAndOperationsClient<TEntity>>();

    /// <summary>
    /// Gets the <see cref="IDynamics365FinanceAndOperationsSecurityContext"/> builder configuration.
    /// </summary>
    public Dynamics365FinanceAndOperationsSecurityContextBuilder SecurityContext
        => _securityContext ??= new Dynamics365FinanceAndOperationsSecurityContextBuilder();

    /// <summary>
    /// Gets the <see cref="IOptions{Dynamics365FinanceAndOperationsSettings}"/> builder configuration.
    /// </summary>
    public OptionsBuilder<Dynamics365FinanceAndOperationsClientSettings> Settings
                => _settings ??= new OptionsBuilder<Dynamics365FinanceAndOperationsClientSettings>();

    /// <inheritdoc/>
    public IDynamics365FinanceAndOperationsClient<TEntity> Build()
    {
        return _settings is null || !_settings.HasValue
            ? BuildMock().Object
            : new Dynamics365FinanceAndOperationsClient<TEntity>(
                HttpClientfactory.Build(),
                SecurityContext.Build(),
                Settings.Build(),
                Logger.Build());
    }

    /// <inheritdoc/>
    public IMock<IDynamics365FinanceAndOperationsClient<TEntity>> BuildMock()
    {
        Mock<IDynamics365FinanceAndOperationsClient<TEntity>> client = new();
        return client;
    }

    /// <summary>
    /// Sets the settings value used for the mocked client.
    /// </summary>
    /// <param name="settings">The settings value.</param>
    /// <returns>The client builder.</returns>
    public Dynamics365FinanceAndOperationsClientBuilder<TEntity> WithSettingsValue(Dynamics365FinanceAndOperationsClientSettings settings)
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
    public Dynamics365FinanceAndOperationsClientBuilder<TEntity> WithValueFromConfiguration<TProgram>()
        where TProgram : class
    {
        _ = Settings.WithValueFromConfiguration<TProgram>();
        _ = SecurityContext.Settings.WithValueFromConfiguration<TProgram>();
        return this;
    }
}