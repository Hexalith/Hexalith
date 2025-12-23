// <copyright file="AssertionHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.TestMocks;

using System.Runtime.Serialization;
using System.Text.Json;

using Shouldly;

/// <summary>
/// Class AssertionHelper.
/// </summary>
public static class AssertionHelper
{
    /// <summary>
    /// Asserts that the object is DataContract serializable.
    /// </summary>
    /// <param name="obj">The object to test.</param>
    /// <param name="customMessage">The custom message.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static void ShouldBeDataContractSerializable(this object obj, string? customMessage = null)
    {
        ArgumentNullException.ThrowIfNull(obj);
        DataContractSerializer serializer = new(obj.GetType());
        using MemoryStream stream = new();
        serializer.WriteObject(stream, obj);
        _ = stream.Seek(0, SeekOrigin.Begin);
        object? result = serializer.ReadObject(stream);
        result.ShouldNotBeNull(customMessage);
        result.ShouldBeEquivalentTo(obj, customMessage);
    }

    /// <summary>
    /// Asserts that the object is JSON serializable.
    /// </summary>
    /// <param name="obj">The object to test.</param>
    /// <param name="customMessage">The custom message.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static void ShouldBeJsonSerializable(this object obj, string? customMessage = null)
    {
        ArgumentNullException.ThrowIfNull(obj);
        string json = JsonSerializer.Serialize(obj);
        object? result = JsonSerializer.Deserialize(json, obj.GetType());
        result.ShouldNotBeNull(customMessage);
        result.ShouldBeEquivalentTo(obj, customMessage);
    }
}