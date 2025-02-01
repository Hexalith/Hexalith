// <copyright file="RelationActorHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Helpers;

using System;

using Dapr.Actors.Runtime;

using Hexalith.Domain.Aggregates;
using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Provides helper methods for managing actor relations.
/// </summary>
public static class RelationActorHelper
{
    /// <summary>
    /// Adds a factory for creating actor relations between two aggregates.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left aggregate.</typeparam>
    /// <typeparam name="TRight">The type of the right aggregate.</typeparam>
    /// <param name="services">The service collection to add the factory to.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddActorRelationFactory<TLeft, TRight>(this IServiceCollection services)
        where TLeft : IDomainAggregate, new()
        where TRight : IDomainAggregate, new()
    {
        ArgumentNullException.ThrowIfNull(services);
        services.TryAddTransient<OneToManyAggregateRelationService<TLeft, TRight>>();
        return services;
    }

    /// <summary>
    /// Gets the name of the actor relation between two aggregates.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left aggregate.</typeparam>
    /// <typeparam name="TRight">The type of the right aggregate.</typeparam>
    /// <returns>The name of the actor relation.</returns>
    public static string GetAggregateRelationActorName<TLeft, TRight>()
        where TLeft : IDomainAggregate, new()
        where TRight : IDomainAggregate, new()
    {
        TLeft left = new();
        TRight right = new();
        return left.AggregateName + right.AggregateName;
    }

    /// <summary>
    /// Registers an actor relation between two aggregates.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left aggregate.</typeparam>
    /// <typeparam name="TRight">The type of the right aggregate.</typeparam>
    /// <param name="actorRegistrationCollection">The actor registration collection to add the relation to.</param>
    public static void RegisterAggregateRelationActor<TLeft, TRight>(this ActorRegistrationCollection actorRegistrationCollection)
        where TLeft : IDomainAggregate, new()
        where TRight : IDomainAggregate, new()
    {
        ArgumentNullException.ThrowIfNull(actorRegistrationCollection);
        string actorName = GetAggregateRelationActorName<TLeft, TRight>();
        if (actorRegistrationCollection.Any(p => p.Type.ActorTypeName == actorName))
        {
            return;
        }

        actorRegistrationCollection.RegisterActor<KeyValueActor>(actorName);
    }
}