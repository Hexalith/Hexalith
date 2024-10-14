// <copyright file="IServerApplication.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Hexalith.Application.Modules.Applications;

using System.Reflection;

/// <summary>
/// Represents the definition of an application.
/// </summary>
public interface IServerApplication : IApplication
{
    /// <summary>
    /// Gets the client application type.
    /// Must be a type that implements <see cref="IClientApplication"/>.
    /// </summary>
    Type ClientApplicationType { get; }

    /// <summary>
    /// Gets the presentation assemblies associated with the application.
    /// </summary>
    public IEnumerable<Assembly> PresentationAssemblies { get; }

    /// <summary>
    /// Gets the server modules associated with the application.
    /// </summary>
    IEnumerable<Type> ServerModules { get; }

    /// <summary>
    /// Gets the shared application type.
    /// Must be a type that implements <see cref="ISharedApplication"/>.
    /// </summary>
    Type SharedApplicationType { get; }

    /// <summary>
    /// Registers the actors associated with the application.
    /// </summary>
    /// <param name="actors">The actor collection.</param>
    void RegisterActors(object actors);
}