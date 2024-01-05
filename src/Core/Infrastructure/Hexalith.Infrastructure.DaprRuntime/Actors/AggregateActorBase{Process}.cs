// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.Sales
// Author           : Jérôme Piquot
// Created          : 01-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-03-2024
// ***********************************************************************
// <copyright file="AggregateActorBase{Process}.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.DaprRuntime.Sales.Actors;

using System;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors;

using Hexalith.Application.Commands;
using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Domain.Messages;
using Hexalith.Infrastructure.DaprRuntime.Abstractions;
using Hexalith.Infrastructure.DaprRuntime.Abstractions.Actors;
using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.Handlers;

/// <summary>
/// Logistics partner catalog item aggregate actor interface <see cref="BspkSalesInvoice" />.
/// Extends the <see cref="IActor" />.
/// </summary>
public abstract partial class AggregateActorBase
{
    /// <inheritdoc/>
    public async Task SubmitCommandAsync(ActorCommandEnvelope envelope)
    {
        ArgumentNullException.ThrowIfNull(envelope);
        Application.Commands.BaseCommand[] commands = envelope.Commands.ToArray();
        Application.Metadatas.BaseMetadata[] metadatas = envelope.Metadatas.ToArray();
        if (commands.Length != metadatas.Length)
        {
            throw new InvalidOperationException($"Invalid commands envelope submitted to {Host.ActorTypeInfo.ActorTypeName}/{Id}. Command and Metadata arrays must have the same number of elements.");
        }

        if (commands.Length == 0)
        {
            LogNoCommandsToSubmitWarning(Logger, Id.ToString(), Host.ActorTypeInfo.ActorTypeName);
            return;
        }

        if (_state is null)
        {
            Dapr.Actors.Runtime.ConditionalValue<AggregateActorState> result = await StateManager
                .TryGetStateAsync<AggregateActorState>(nameof(AggregateActorState))
                .ConfigureAwait(false);
            _state = result.HasValue ? result.Value : new AggregateActorState();
        }

        List<CommandState> commandStates = [];
        for (int i = 0; i < commands.Length; i++)
        {
            Application.Commands.BaseCommand command = commands[i];
            Application.Metadatas.BaseMetadata metadata = metadatas[i];
            if ((command.AggregateName + nameof(Aggregate)) != Host.ActorTypeInfo.ActorTypeName)
            {
                throw new InvalidOperationException($"Submitted command to {Host.ActorTypeInfo.ActorTypeName}/{Id} has an invalid aggregate name : {command.AggregateName}.");
            }

            if (command.AggregateId != Id.ToString())
            {
                throw new InvalidOperationException($"Submitted command to {Host.ActorTypeInfo.ActorTypeName}/{Id} has an invalid aggregate id : {command.AggregateId}.");
            }

            CommandState commandState = new(_dateTimeService.UtcNow, command, metadata);
            commandStates.Add(commandState);
            LogAcceptedCommandInformation(
                Logger,
                command.TypeName,
                metadata.Message.Id,
                command.AggregateName,
                command.AggregateId);
        }

        _state.CommandCount = await CommandStore.AddAsync(commandStates, _state.CommandCount, CancellationToken.None).ConfigureAwait(false);

        _state.Reminder = TimeSpan.FromMinutes(1);
        _ = await RegisterReminderAsync(
            ActorConstants.ContinueReminderName,
            null,
            _state.Reminder.Value,
            _state.Reminder.Value)
            .ConfigureAwait(false);
        await StateManager.SetStateAsync(nameof(AggregateActorState), _state).ConfigureAwait(false);
        await StateManager.SaveStateAsync().ConfigureAwait(false);
        await RegisterContinueTimerAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    Task<bool> IAggregateActor.ProcessNextCommandAsync() => throw new NotImplementedException();

    private async Task<(long EventSourceVersion, long MessageStoreVersion)> ProcessCommandAsync(
        CommandState commandState,
        long eventSourceVersion,
        long messageStoreVersion,
        CancellationToken cancellationToken)
    {
        BaseCommand command = commandState.Message ?? throw new InvalidOperationException("The specified command state is missing associated message.");
        BaseMetadata metadata = commandState.Metadata ?? throw new InvalidOperationException("The specified command state is missing associated metadata.");

        if (command.AggregateId != Id.ToString())
        {
            throw new InvalidOperationException($"Command {command.TypeName} for {Id} has an invalid aggregate id : {commandState.Message.AggregateId}.");
        }

        IAggregate aggregate = await GetAggregateAsync(commandState.Message.AggregateName, CancellationToken.None)
                .ConfigureAwait(false);

        // Execute the commands
        IEnumerable<BaseMessage> messages = await _commandDispatcher
                .DoAsync(command, aggregate, cancellationToken)
                .ConfigureAwait(false);

        // Get aggregate events to persist in the event sourcing store
        BaseEvent[] aggregateEvents = messages
            .OfType<BaseEvent>()
            .Where(p => p.AggregateId == command.AggregateId && p.AggregateName == aggregate.AggregateName)
            .ToArray();
        EventState[] aggregateEventStates = aggregateEvents
            .Select(p => new EventState(_dateTimeService.UtcNow, p, Metadata.CreateNew(p, metadata)))
            .ToArray();

        // Get integration messages to persist in the message store
        MessageState[] integrationMessages = messages
            .OfType<BaseMessage>()
            .Where(p => !p.IsPrivateToAggregate)
            .Select(p => new MessageState(_dateTimeService.UtcNow, p, Metadata.CreateNew(p, metadata)))
            .ToArray();

        // Apply events to aggregate
        _aggregate = aggregate.Apply(aggregateEvents);

        // Persist events and messages
        long newEventSourceVersion = await EventSourceStore.AddAsync(aggregateEventStates, eventSourceVersion, cancellationToken).ConfigureAwait(false);
        long newMessageStoreVersion = await MessageStore.AddAsync(integrationMessages, messageStoreVersion, cancellationToken).ConfigureAwait(false);

        LogProcessedCommandInformation(
            Logger,
            command.TypeName,
            metadata.Message.Id,
            command.AggregateName,
            command.AggregateId);
        return (newEventSourceVersion, newMessageStoreVersion);
    }

    private async Task<bool> ProcessNextSubmittedCommandAsync(CancellationToken cancellationToken)
    {
        AggregateActorState state = await GetStateAsync(cancellationToken)
            .ConfigureAwait(false);

        LogProcessingCommandsInformation(Logger, Id.ToString(), Host.ActorTypeInfo.ActorTypeName, state.CommandCount, state.LastCommandProcessed);
        if (state.LastCommandProcessed < state.CommandCount)
        {
            // Get the next command to process
            CommandState commandState = await CommandStore
                .GetAsync(++state.LastCommandProcessed, CancellationToken.None)
                .ConfigureAwait(false);
            (long eventSourceVersion, long messageStoreVersion) = await ProcessCommandAsync(
                commandState,
                state.EventCount,
                state.MessageCount,
                cancellationToken).ConfigureAwait(false);

            state.MessageCount = messageStoreVersion;
            state.EventCount = eventSourceVersion;
            await StateManager.SetStateAsync(nameof(AggregateActorState), state, cancellationToken).ConfigureAwait(false);
            await StateManager.SaveStateAsync(cancellationToken).ConfigureAwait(false);
            return true;
        }

        return false;
    }
}