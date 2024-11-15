// <copyright file="IApplication.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.Application.Modules.Applications;

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
    public IApiServerApplication ApiServerApplication { get; }

    /// <summary>
    /// Gets the home path of the application.
    /// </summary>
    string HomePath { get; }

    /// <summary>
    /// Gets the unique identifier of the application.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets a value indicating whether tHe application is a client.
    /// </summary>
    public bool IsClient { get; }

    /// <summary>
    /// Gets a value indicating whether the application is a server.
    /// </summary>
    public bool IsServer { get; }

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
    public string Name { get; }

    /// <summary>
    /// Gets the session cookie name.
    /// </summary>
    public string SessionCookieName { get; }

    /// <summary>
    /// Gets the shared application.
    /// </summary>
    /// <returns>The application instance.</returns>
    /// <exception cref="InvalidOperationException">No application found.</exception>
    public ISharedUIElementsApplication SharedUIElementsApplication { get; }

    /// <summary>
    /// Gets the version of the application.
    /// </summary>
    public string Version { get; }

    /// <summary>
    /// Gets the client application.
    /// </summary>
    /// <returns>The application instance.</returns>
    /// <exception cref="InvalidOperationException">No application found.</exception>
    public IWebAppApplication WebAppApplication { get; }

    /// <summary>
    /// Gets the server application.
    /// </summary>
    /// <returns>The application instance.</returns>
    /// <exception cref="InvalidOperationException">No application found.</exception>
    public IWebServerApplication WebServerApplication { get; }

    /// <summary>
    /// Adds services to the application.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration properties.</param>
    public void AddServices(IServiceCollection services, IConfiguration configuration);
}