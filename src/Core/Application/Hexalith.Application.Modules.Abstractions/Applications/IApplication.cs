// <copyright file="IApplication.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.Application.Modules.Applications;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Represents the definition of an application.
/// </summary>
public interface IApplication
{
    /// <summary>
    /// Gets the server application.
    /// </summary>
    /// <returns>The application instance.</returns>
    /// <exception cref="InvalidOperationException">No application found.</exception>
    IApiServerApplication? ApiServerApplication { get; }

    /// <summary>
    /// Gets the application type.
    /// </summary>
    ApplicationType ApplicationType { get; }

    /// <summary>
    /// Gets the description of the application.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Gets the home path of the application.
    /// </summary>
    string HomePath { get; }

    /// <summary>
    /// Gets the unique identifier of the application.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets the login path of the application.
    /// </summary>
    string LoginPath { get; }

    /// <summary>
    /// Gets the logout path of the application.
    /// </summary>
    string LogoutPath { get; }

    /// <summary>
    /// Gets the modules associated with the application.
    /// </summary>
    IEnumerable<Type> Modules { get; }

    /// <summary>
    /// Gets the name of the application.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the session cookie name.
    /// </summary>
    string SessionCookieName { get; }

    /// <summary>
    /// Gets the short name of the application.
    /// </summary>
    string ShortName { get; }

    /// <summary>
    /// Gets the user account path of the application.
    /// </summary>
    string UserAccountPath { get; }

    /// <summary>
    /// Gets the version of the application.
    /// </summary>
    string Version { get; }

    /// <summary>
    /// Gets the client application.
    /// </summary>
    /// <returns>The application instance.</returns>
    /// <exception cref="InvalidOperationException">No application found.</exception>
    IWebAppApplication? WebAppApplication { get; }

    /// <summary>
    /// Gets the server application.
    /// </summary>
    /// <returns>The application instance.</returns>
    /// <exception cref="InvalidOperationException">No application found.</exception>
    IWebServerApplication? WebServerApplication { get; }

    /// <summary>
    /// Adds services to the application.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration properties.</param>
    void AddServices(IServiceCollection services, IConfiguration configuration);

    /// <summary>
    /// Configures the authorization options for the application.
    /// </summary>
    /// <returns>An action to configure <see cref="AuthorizationOptions"/>.</returns>
    Action<AuthorizationOptions> ConfigureAuthorization();
}