// <copyright file="VersionHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Helpers;

using System;
using System.Diagnostics;

/// <summary>
/// Provides helper methods for retrieving the product version of types and assemblies.
/// </summary>
public static class VersionHelper
{
    /// <summary>
    /// Retrieves the product version of the specified object's type.
    /// </summary>
    /// <param name="obj">The object whose type's product version should be retrieved.</param>
    /// <returns>The product version of the object's type.</returns>
    public static string? ProductVersion(this object obj) => (obj == null) ? null : ProductVersion(obj.GetType());

    /// <summary>
    /// Retrieves the product version of the specified type.
    /// </summary>
    /// <typeparam name="T">The type whose product version should be retrieved.</typeparam>
    /// <returns>The product version of the specified type.</returns>
    public static string? ProductVersion<T>() => ProductVersion(typeof(T));

    /// <summary>
    /// Retrieves the product version of the specified type.
    /// </summary>
    /// <param name="type">The type whose product version should be retrieved.</param>
    /// <returns>The product version of the specified type.</returns>
    public static string? ProductVersion(Type type)
    {
        System.Reflection.Assembly? assembly = type?.Assembly;
        return assembly == null ? null : FileProductVersion(assembly.Location);
    }

    /// <summary>
    /// Retrieves the product version of the entry assembly.
    /// </summary>
    /// <returns>The product version of the entry assembly.</returns>
    public static string? EntryProductVersion()
    {
        System.Reflection.Assembly? assembly = System.Reflection.Assembly.GetEntryAssembly();
        return assembly == null ? null : FileProductVersion(assembly.Location);
    }

    /// <summary>
    /// Retrieves the product version of the file at the specified location.
    /// </summary>
    /// <param name="location">The location of the file.</param>
    /// <returns>The product version of the file.</returns>
    public static string? FileProductVersion(string location) => FileVersionInfo.GetVersionInfo(location).ProductVersion ?? "?.?.?";
}