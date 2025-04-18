// <copyright file="SimpleCommandHandler{TCommand}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Domain.Aggregates;
using Hexalith.PolymorphicSerializations;

using Microsoft.Extensions.Logging;

/// <summary>
/// Command handler for domain commands.
/// </summary>
/// <typeparam name="TCommand">The command type.</typeparam>
public class SimpleCommandHandler<TCommand>(
    Func<TCommand, Polymorphic> toEvent,
    TimeProvider timeProvider,
    ILogger<DomainCommandHandler<TCommand>> logger) : DomainCommandHandler<TCommand>(timeProvider, logger)
{
    /// <summary>
    /// Gets the function to convert a command to an event.
    /// </summary>
    protected Func<TCommand, Polymorphic> ToEvent { get; } = toEvent;

    /// <inheritdoc/>
    public override async Task<ExecuteCommandResult> DoAsync(TCommand command, Metadata metadata, IDomainAggregate? aggregate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        Polymorphic ev = ToEvent(command);
        return await Task.FromResult(
            CheckAggregateIsValid<IDomainAggregate>(aggregate, metadata)
            .Apply(ev)
            .CreateCommandResult(ev, metadata, Time));
    }
}