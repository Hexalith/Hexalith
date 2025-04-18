// <copyright file="SimpleInitializationCommandHandler{TCommand}.cs" company="ITANEO">
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
public class SimpleInitializationCommandHandler<TCommand>(
    Func<TCommand, Polymorphic> toEvent,
    Func<Polymorphic, IDomainAggregate> initializeAggregate,
    TimeProvider timeProvider,
    ILogger<DomainCommandHandler<TCommand>> logger)
    : SimpleCommandHandler<TCommand>(toEvent, timeProvider, logger)
{
    /// <inheritdoc/>
    public override Task<ExecuteCommandResult> DoAsync(TCommand command, Metadata metadata, IDomainAggregate? aggregate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (aggregate is null || string.IsNullOrWhiteSpace(aggregate.AggregateId))
        {
            Polymorphic ev = ToEvent(command);
            aggregate = initializeAggregate(ev);
            return Task.FromResult(new ExecuteCommandResult(aggregate, [ev], [ev], false));
        }

        return base.DoAsync(command, metadata, aggregate, cancellationToken);
    }
}