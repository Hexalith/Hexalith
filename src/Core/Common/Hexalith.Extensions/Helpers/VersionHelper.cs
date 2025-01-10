// <copyright file="VersionHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Helpers;

using System;
using System.Diagnostics;
using System.Reflection;

/// <summary>
/// Provides helper methods for retrieving the product version of types and assemblies.
/// </summary>
public static class VersionHelper
{
    /// <summary>
    /// Retrieves the product version of the entry assembly.
    /// </summary>
    /// <returns>The product version of the entry assembly.</returns>
    public static string EntryProductVersion()
    {
        Assembly? assembly = Assembly.GetEntryAssembly();
        return assembly is null ? throw new InvalidOperationException("Entry assembly not found.") : assembly.GetAssemblyVersion();
    }

    /// <summary>
    /// Retrieves the product version of the file at the specified location.
    /// </summary>
    /// <param name="location">The location of the file.</param>
    /// <returns>The product version of the file.</returns>
    public static string? FileProductVersion(string location) => FileVersionInfo.GetVersionInfo(location).ProductVersion;

    /// <summary>
    /// Retrieves the version of the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly whose version is to be retrieved.</param>
    /// <returns>The version of the specified assembly.</returns>
    public static string GetAssemblyVersion(this Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        string? version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;
        return version?.Split('+')[0] ?? "0.0.1-dev";
    }

    /// <summary>
    /// Retrieves the product version of the specified type.
    /// </summary>
    /// <param name="type">The type whose product version should be retrieved.</param>
    /// <returns>The product version of the specified type.</returns>
    public static string GetAssemblyVersion(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return type.Assembly.GetAssemblyVersion();
    }
}