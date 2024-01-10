// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.Sales
// Author           : Jérôme Piquot
// Created          : 01-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-03-2024
// ***********************************************************************
// <copyright file="AggregateActorBase{Publish}.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.DaprRuntime.Sales.Actors;

using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors;

using Hexalith.Application.Commands;
using Hexalith.Application.Notifications;
using Hexalith.Application.Requests;
using Hexalith.Application.States;
using Hexalith.Domain.Events;
using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Actors;

using Microsoft.Extensions.Logging;

/// <summary>
/// Logistics partner catalog item aggregate actor interface <see cref="BspkSalesInvoice" />.
/// Extends the <see cref="IActor" />.
/// </summary>
public abstract partial class AggregateActorBase
{
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

    [LoggerMessage(
            EventId = 6,
            Level = LogLevel.Warning,
            Message = "Publish message {MessageSequence} (Id={MessageId}; CorrelationId={CorrelationId}) operation failed on actor {ActorType}/{ActorId}. Error : {ErrorMessage}")]
    public static partial void LogPublishError(ILogger logger, Exception exception, long messageSequence, string? messageId, string? correlationId, string actorId, string actorType, string errorMessage);

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
            await SetPublishCallbackAsync(cancellationToken)
                .ConfigureAwait(false);
            await SaveAggregateStateAsync(cancellationToken)
                .ConfigureAwait(false);
            await SaveStateAsync()
                .ConfigureAwait(false);
        }

        return state.LastMessagePublished < state.MessageCount;
    }

    private async Task PublishNextEmittedMessageAsync(MessageState messageState, long sequence, CancellationToken cancellationToken)
    {
        AggregateActorState state = await GetAggregateStateAsync(cancellationToken).ConfigureAwait(false);
        state.PublishFailed = false;
#pragma warning disable CA1031 // Do not catch general exception types
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
#pragma warning restore CA1031 // Do not catch general exception types

        state.LastMessagePublished = sequence;
        LogInvalidPublishMessageError(
            Logger,
            sequence,
            messageState.Metadata?.Message?.Id,
            messageState.Metadata?.Message.Name,
            messageState.Metadata?.Context.CorrelationId,
            messageState.Metadata?.Message.Aggregate.Id);
    }
}