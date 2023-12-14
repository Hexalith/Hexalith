// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.TestMocks
// Author           : Jérôme Piquot
// Created          : 10-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-07-2023
// ***********************************************************************
// <copyright file="Dynamics365FinanceClientBuilder{TEntity}.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.TestMocks;

using Hexalith.Application.Organizations.Configurations;
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
    where TEntity : class, IODataCommon
{
    /// <summary>
    /// The settings.
    /// </summary>
    private OptionsBuilder<Dynamics365FinanceClientSettings>? _finOpsSettings;

    /// <summary>
    /// The logger.
    /// </summary>
    private LoggerBuilder<Dynamics365FinanceClient<TEntity>>? _logger;

    /// <summary>
    /// The organization settings.
    /// </summary>
    private OptionsBuilder<OrganizationSettings>? _organizationSettings;

    /// <summary>
    /// The security context.
    /// </summary>
    private Dynamics365FinanceSecurityContextBuilder? _securityContext;

    /// <summary>
    /// Gets the <see cref="IOptions{Dynamics365FinanceSettings}" /> builder configuration.
    /// </summary>
    /// <value>The settings.</value>
    public OptionsBuilder<Dynamics365FinanceClientSettings> FinOpsSettings
                => _finOpsSettings ??= new OptionsBuilder<Dynamics365FinanceClientSettings>();

    /// <summary>
    /// Gets the <see cref="IHttpClientFactory" /> builder configuration.
    /// </summary>
    /// <value>The HTTP clientfactory.</value>
    public LoggerBuilder<Dynamics365FinanceClient<TEntity>> Logger
            => _logger ??= new LoggerBuilder<Dynamics365FinanceClient<TEntity>>();

    /// <summary>
    /// Gets the organization settings.
    /// </summary>
    /// <value>The organization settings.</value>
    public OptionsBuilder<OrganizationSettings> OrganizationSettings
                => _organizationSettings ??= new OptionsBuilder<OrganizationSettings>();

    /// <summary>
    /// Gets the <see cref="IDynamics365FinanceSecurityContext" /> builder configuration.
    /// </summary>
    /// <value>The security context.</value>
    public Dynamics365FinanceSecurityContextBuilder SecurityContext
        => _securityContext ??= new Dynamics365FinanceSecurityContextBuilder();

    /// <inheritdoc/>
    public IDynamics365FinanceClient<TEntity> Build(HttpClient httpClient)
    {
        return _finOpsSettings is null || !_finOpsSettings.HasValue
            ? BuildMock().Object
            : new Dynamics365FinanceClient<TEntity>(
                httpClient,
                SecurityContext.Build(),
                FinOpsSettings.Build(),
                OrganizationSettings.Build(),
                Logger.Build());
    }

    /// <summary>
    /// Build a mocked instance of <typeparamref name="T" />.
    /// </summary>
    /// <returns>The mocked instance.</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public IDynamics365FinanceClient<TEntity> Build() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IMock<IDynamics365FinanceClient<TEntity>> BuildMock()
    {
        Mock<IDynamics365FinanceClient<TEntity>> client = new();
        return client;
    }

    /// <summary>
    /// Withes the settings value.
    /// </summary>
    /// <param name="finOpsSettings">The fin ops settings.</param>
    /// <param name="organizationSettings">The organization settings.</param>
    /// <returns>Dynamics365FinanceClientBuilder&lt;TEntity&gt;.</returns>
    public Dynamics365FinanceClientBuilder<TEntity> WithSettingsValue(Dynamics365FinanceClientSettings finOpsSettings, OrganizationSettings organizationSettings)
    {
        _ = FinOpsSettings.WithValue(finOpsSettings);
        _ = OrganizationSettings.WithValue(organizationSettings);
        _ = SecurityContext.Settings.WithValue(finOpsSettings);
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
        _ = FinOpsSettings.WithValueFromConfiguration<TProgram>();
        _ = OrganizationSettings.WithValueFromConfiguration<TProgram>();
        _ = SecurityContext.Settings.WithValueFromConfiguration<TProgram>();
        return this;
    }
}