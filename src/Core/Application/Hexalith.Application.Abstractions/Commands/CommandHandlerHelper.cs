// <copyright file="CommandHandlerHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Commands;

using System;

using Hexalith.Application.Events;
using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Domains;
using Hexalith.Domains.Results;
using Hexalith.PolymorphicSerializations;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

/// <summary>
/// Provides helper methods for handling commands.
/// </summary>
public static class CommandHandlerHelper
{
    /// <summary>
    /// Creates the command result based on the apply result.
    /// </summary>
    /// <param name="result">The result of applying the command.</param>
    /// <param name="ev">The event associated with the command.</param>
    /// <param name="metadata">The metadata associated with the event.</param>
    /// <param name="timeProvider">The time provider to get the current time.</param>
    /// <returns>The result of executing the command.</returns>
    public static ExecuteCommandResult CreateCommandResult(this ApplyResult result, Polymorphic ev, Metadata metadata, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ev);
        ArgumentNullException.ThrowIfNull(metadata);
        ArgumentNullException.ThrowIfNull(timeProvider);
        return result.Failed
                ? new ExecuteCommandResult(
                    result.Aggregate,
                    [],
                    [
                        new DomainEventCancelled(
                            result.Reason ?? "Unknown reason",
                            new MessageState(ev, Metadata.CreateNew(ev, metadata, timeProvider.GetLocalNow()))),
                    ],
                    result.Failed)
                : new ExecuteCommandResult(
                    result.Aggregate,
                    [ev],
                    result.Messages,
                    result.Failed);
    }

    /// <summary>
    /// Adds a simple command handler to the service collection.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="toEvent">The function to convert the command to an event.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection TryAddSimpleCommandHandler<TCommand>(this IServiceCollection services, Func<TCommand, Polymorphic> toEvent)
    {
        services.TryAddSingleton<IDomainCommandHandler<TCommand>>(s =>
            new SimpleCommandHandler<TCommand>(
                toEvent,
                s.GetRequiredService<TimeProvider>(),
                s.GetRequiredService<ILogger<DomainCommandHandler<TCommand>>>()));
        return services;
    }

    /// <summary>
    /// Adds a simple initialization command handler to the service collection.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="toEvent">The function to convert the command to an event.</param>
    /// <param name="toAggregate">The function to convert the event to an aggregate.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection TryAddSimpleInitializationCommandHandler<TCommand>(
        this IServiceCollection services,
        Func<TCommand, Polymorphic> toEvent,
        Func<Polymorphic, IDomainAggregate> toAggregate)
    {
        services.TryAddSingleton<IDomainCommandHandler<TCommand>>(s =>
            new SimpleInitializationCommandHandler<TCommand>(
                toEvent,
                toAggregate,
                s.GetRequiredService<TimeProvider>(),
                s.GetRequiredService<ILogger<DomainCommandHandler<TCommand>>>()));
        return services;
    }
}