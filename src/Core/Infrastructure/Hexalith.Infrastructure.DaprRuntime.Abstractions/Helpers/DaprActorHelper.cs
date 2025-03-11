// <copyright file="DaprActorHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Helpers;

using Dapr.Actors;
using Dapr.Actors.Client;
using Dapr.Actors.Runtime;

using Hexalith.Application.Metadatas;
using Hexalith.Infrastructure.DaprRuntime.Actors;

/// <summary>
/// Provides helper methods for working with Dapr Actors, specifically for converting between string identifiers and ActorId objects.
/// </summary>
/// <remarks>
/// This class contains static methods that facilitate the conversion between string-based identifiers and Dapr's ActorId objects.
/// It ensures proper escaping and unescaping of identifiers to maintain compatibility with Dapr's actor model.
/// </remarks>
public static class DaprActorHelper
{
    private const string _actorSuffix = "Aggregate";

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
    {
        ArgumentNullException.ThrowIfNull(id);
        string sanitizedId = id
            .Replace("!", "!0")
            .Replace(" ", "!1")
            .Replace("/", "!2");
        sanitizedId = Uri.EscapeDataString(sanitizedId.Trim());
        return new ActorId(sanitizedId);

        // return sanitizedId.IsRfc1123Compliant()
        //    ? new ActorId(sanitizedId)
        //    : throw new ArgumentException($"The sanitized identifier '{sanitizedId}' is not RFC 1123 compliant. Original : '{id}'.");
    }

    /// <summary>
    /// Gets the name of the aggregate actor.
    /// </summary>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <returns>string.</returns>
    public static string ToAggregateActorName(this string aggregateName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(aggregateName);
        return aggregateName + _actorSuffix;
    }

    /// <summary>
    /// Converts an ActorTypeInformation to its aggregate name.
    /// </summary>
    /// <param name="typeName">The ActorTypeInformation to convert.</param>
    /// <returns>The aggregate name as a string.</returns>
    public static string ToAggregateName(this ActorTypeInformation typeName)
        => ToAggregateName(typeName.ActorTypeName);

    /// <summary>
    /// Converts an actor type name to its aggregate name.
    /// </summary>
    /// <param name="actorTypeName">The actor type name to convert.</param>
    /// <returns>The aggregate name as a string.</returns>
    /// <exception cref="ArgumentException">Thrown when the actor type name does not end with the expected suffix.</exception>
    public static string ToAggregateName(string actorTypeName)
    {
        // Verify that the actor type name contains the aggregate suffix.
        return actorTypeName.EndsWith(_actorSuffix)
            ? actorTypeName[..^_actorSuffix.Length]
            : throw new ArgumentException($"The actor type name '{actorTypeName}' does not end with the expected suffix '{_actorSuffix}'.");
    }

    /// <summary>
    /// Creates a proxy for a domain aggregate actor.
    /// </summary>
    /// <param name="actorProxy">The actor proxy factory.</param>
    /// <param name="aggregateName">The name of the aggregate.</param>
    /// <param name="aggregateGlobalId">The identifier of the aggregate.</param>
    /// <param name="timeout">The optional request timeout.</param>
    /// <returns>An <see cref="IDomainAggregateActor"/> proxy.</returns>
    /// <remarks>
    /// This method creates a proxy for a domain aggregate actor using the specified actor proxy factory, aggregate name, and aggregate identifier.
    /// It allows for an optional request timeout to be specified.
    /// </remarks>
    public static IDomainAggregateActor ToDomainAggregateActor(this IActorProxyFactory actorProxy, string aggregateName, string aggregateGlobalId, TimeSpan? timeout = null)
    {
        ArgumentNullException.ThrowIfNull(aggregateGlobalId);
        return actorProxy.CreateActorProxy<IDomainAggregateActor>(
            aggregateGlobalId.ToActorId(),
            aggregateName.ToAggregateActorName(),
            new ActorProxyOptions { RequestTimeout = timeout });
    }

    /// <summary>
    /// Creates a proxy for a domain aggregate actor using metadata.
    /// </summary>
    /// <param name="actorProxy">The actor proxy factory.</param>
    /// <param name="metadata">The metadata containing aggregate information.</param>
    /// <param name="timeout">The optional request timeout.</param>
    /// <returns>An <see cref="IDomainAggregateActor"/> proxy.</returns>
    /// <remarks>
    /// This method creates a proxy for a domain aggregate actor using the specified actor proxy factory and metadata.
    /// It allows for an optional request timeout to be specified.
    /// </remarks>
    public static IDomainAggregateActor ToDomainAggregateActor(this IActorProxyFactory actorProxy, Metadata metadata, TimeSpan? timeout = null)
    {
        ArgumentNullException.ThrowIfNull(metadata);
        ArgumentException.ThrowIfNullOrWhiteSpace(metadata.AggregateGlobalId);
        ArgumentException.ThrowIfNullOrWhiteSpace(metadata.Message.Aggregate.Name);

        return actorProxy.ToDomainAggregateActor(metadata.Message.Aggregate.Name, metadata.AggregateGlobalId, timeout);
    }

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
    {
        string identifier = Uri.UnescapeDataString(id.ToString());
        identifier = identifier
            .Replace("!2", "/")
            .Replace("!1", " ")
            .Replace("!0", "!");
        return identifier;
    }
}