// <copyright file="DomainAggregateActorBase{Publish}.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Sales.Actors;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Application.MessageMetadatas;
using Hexalith.Application.Requests;
using Hexalith.Domain.Events;
using Hexalith.Domain.Notifications;
using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Abstractions;
using Hexalith.Infrastructure.DaprRuntime.Abstractions.Actors;
using Hexalith.Infrastructure.DaprRuntime.Actors;

using Microsoft.Extensions.Logging;

/// <summary>
/// The aggregate manager actor class.
/// Implements the <see cref="Hexalith.Infrastructure.DaprRuntime.Sales.Actors.AggregateActorBase" />
/// Implements the <see cref="IAggregateActor" />.
/// </summary>
/// <seealso cref="Hexalith.Infrastructure.DaprRuntime.Sales.Actors.AggregateActorBase" />
/// <seealso cref="IAggregateActor" />
public abstract partial class DomainAggregateActorBase
{
    /// <summary>
    /// Logs the invalid publish message error.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="sequence">The sequence.</param>
    /// <param name="messageId">The message identifier.</param>
    /// <param name="messageType">Type of the message.</param>
    /// <param name="correlationId">The correlation identifier.</param>
    /// <param name="aggregateId">The aggregate identifier.</param>
    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Error,
        Message = "Message {Sequence} Id={MessageId} Type={MessageType} CorrelationId={CorrelationId} AggregateId={AggregateId} is invalid and cannot be published.")]
    public static partial void LogInvalidPublishMessageError(
            ILogger logger,
            long sequence,
            string? messageId,
            string? messageType,
            string? correlationId,
            string? aggregateId);

    /// <summary>
    /// Logs the publish error.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="exception">The exception.</param>
    /// <param name="messageSequence">The message sequence.</param>
    /// <param name="messageId">The message identifier.</param>
    /// <param name="correlationId">The correlation identifier.</param>
    /// <param name="actorId">The actor identifier.</param>
    /// <param name="actorType">Type of the actor.</param>
    /// <param name="errorMessage">The error message.</param>
    [LoggerMessage(
            EventId = 6,
            Level = LogLevel.Warning,
            Message = "Publish message {MessageSequence} (Id={MessageId}; CorrelationId={CorrelationId}) operation failed on actor {ActorType}/{ActorId}. Error : {ErrorMessage}")]
    public static partial void LogPublishError(
        ILogger logger,
        Exception exception,
        long messageSequence,
        string? messageId,
        string? correlationId,
        string actorId,
        string actorType,
        string errorMessage);

    /// <inheritdoc/>
    public async Task PublishCallbackAsync() => await PublishNextMessageAsync().ConfigureAwait(false);

    /// <inheritdoc/>
    public async Task<bool> PublishNextMessageAsync()
    {
        CancellationToken cancellationToken = CancellationToken.None;
        AggregateActorState state = await GetAggregateStateAsync(cancellationToken).ConfigureAwait(false);
        if (state.LastMessagePublished < state.MessageCount)
        {
            MessageState messageState = await MessageStore
                .GetAsync(state.LastMessagePublished + 1, cancellationToken)
                .ConfigureAwait(false);
            await PublishNextEmittedMessageAsync(
                    messageState,
                    state.LastMessagePublished + 1,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        await SetPublishCallbackAsync(cancellationToken)
            .ConfigureAwait(false);
        await SaveAggregateStateAsync(cancellationToken)
            .ConfigureAwait(false);
        await SaveStateAsync()
            .ConfigureAwait(false);

        return state.LastMessagePublished < state.MessageCount;
    }

    [SuppressMessage(
        "Design",
        "CA1031:Do not catch general exception types",
        Justification = "All errors must be caught to avoid actor transaction rollback. The message will be republished later.")]
    private async Task PublishNextEmittedMessageAsync(MessageState messageState, long sequence, CancellationToken cancellationToken)
    {
        AggregateActorState state = await GetAggregateStateAsync(cancellationToken).ConfigureAwait(false);
        state.PublishFailed = false;
        try
        {
            if (messageState.Message is BaseEvent ev)
            {
                await _eventBus.PublishAsync(new EventState(messageState.ReceivedDate, ev, messageState.Metadata), cancellationToken).ConfigureAwait(false);
                state.LastMessagePublished = sequence;
                return;
            }

            if (messageState.Message is BaseNotification notification)
            {
                await _notificationBus.PublishAsync(new NotificationState(messageState.ReceivedDate, notification, messageState.Metadata), cancellationToken).ConfigureAwait(false);
                state.LastMessagePublished = sequence;
                return;
            }

            if (messageState.Message is BaseCommand command)
            {
                await _commandBus.PublishAsync(new CommandState(messageState.ReceivedDate, command, messageState.Metadata), cancellationToken).ConfigureAwait(false);
                state.LastMessagePublished = sequence;
                return;
            }

            if (messageState.Message is BaseRequest request)
            {
                await _requestBus.PublishAsync(new RequestState(messageState.ReceivedDate, request, messageState.Metadata), cancellationToken).ConfigureAwait(false);
                state.LastMessagePublished = sequence;
                return;
            }
        }
        catch (Exception ex)
        {
            state.PublishFailed = true;
            LogPublishError(
                Logger,
                ex,
                sequence,
                messageState.Metadata?.Message.Id,
                messageState.Metadata?.Context.CorrelationId,
                Id.ToString(),
                Host.ActorTypeInfo.ActorTypeName,
                ex.FullMessage());
            return;
        }

        state.LastMessagePublished = sequence;
        LogInvalidPublishMessageError(
            Logger,
            sequence,
            messageState.Metadata?.Message?.Id,
            messageState.Metadata?.Message.Name,
            messageState.Metadata?.Context.CorrelationId,
            messageState.Metadata?.Message.Aggregate.Id);
    }

    private async Task SetPublishCallbackAsync(CancellationToken cancellationToken)
    {
        AggregateActorState state = await GetAggregateStateAsync(cancellationToken).ConfigureAwait(false);
        TimeSpan timerWaitTime = timerWaitTime = state.PublishFailed ? _maxTimerDueTime : _defaultTimerDueTime;
        TimeSpan reminderWaitTime = _maxTimerDueTime;

        if (state.LastMessagePublished < state.MessageCount)
        {
            if (state.PublishReminderDueTime == null)
            {
                _ = await RegisterReminderAsync(ActorConstants.PublishReminderName, null, reminderWaitTime, reminderWaitTime).ConfigureAwait(false);
                state.PublishReminderDueTime = reminderWaitTime;
            }

            _ = await RegisterTimerAsync(ActorConstants.PublishTimerName, nameof(PublishCallbackAsync), null, timerWaitTime, TimeSpan.FromMilliseconds(-1)).ConfigureAwait(false);
        }
        else
        {
            if (state.PublishReminderDueTime is not null)
            {
                await UnregisterReminderAsync(ActorConstants.PublishReminderName).ConfigureAwait(false);
                state.PublishReminderDueTime = null;
            }
        }
    }
}