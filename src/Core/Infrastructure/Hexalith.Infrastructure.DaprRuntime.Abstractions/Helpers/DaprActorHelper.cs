// <copyright file="DaprActorHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Helpers;

using Dapr.Actors;

/// <summary>
/// Provides helper methods for working with Dapr Actors, specifically for converting between string identifiers and ActorId objects.
/// </summary>
/// <remarks>
/// This class contains static methods that facilitate the conversion between string-based identifiers and Dapr's ActorId objects.
/// It ensures proper escaping and unescaping of identifiers to maintain compatibility with Dapr's actor model.
/// </remarks>
public static class DaprActorHelper
{
    /// <summary>
    /// Converts a string identifier to an ActorId.
    /// </summary>
    /// <param name="id">The string identifier to convert.</param>
    /// <returns>An <see cref="ActorId"/> created from the escaped string identifier.</returns>
    /// <remarks>
    /// This method escapes the input string to ensure it's a valid ActorId.
    /// It uses Uri.EscapeDataString to handle special characters that might be present in the identifier.
    /// This is particularly useful when working with identifiers that may contain characters not allowed in ActorIds.
    /// </remarks>
    public static ActorId ToActorId(this string id)
        => new(Uri.EscapeDataString(id.Trim()));

    /// <summary>
    /// Converts an ActorId to its original, unescaped string representation.
    /// </summary>
    /// <param name="id">The ActorId to convert.</param>
    /// <returns>The original, unescaped string representation of the ActorId.</returns>
    /// <remarks>
    /// This method reverses the escaping process performed by ToActorId.
    /// It uses Uri.UnescapeDataString to restore any special characters that were escaped during the ActorId creation.
    /// This is useful when you need to retrieve the original identifier from an ActorId, especially for display or logging purposes.
    /// </remarks>
    public static string ToUnescapeString(this ActorId id)
        => Uri.UnescapeDataString(id.ToString());
}