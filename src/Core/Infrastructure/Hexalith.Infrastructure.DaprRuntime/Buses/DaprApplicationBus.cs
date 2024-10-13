// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprBus
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-05-2023
// ***********************************************************************
// <copyright file="DaprApplicationBus.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Buses;

using System.Text.Json;

using Dapr.Client;

using Hexalith.Application.Envelopes;
using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Domain.Messages;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;

using Microsoft.Extensions.Logging;

/// <summary>
/// This class is used to store events in a Dapr actor state.
/// </summary>
/// <typeparam name="TMessage">The type of the state.Message.</typeparam>
/// <typeparam name="TMetadata">The type of the state.Metadata.</typeparam>
/// <typeparam name="TState">The type of the t state.</typeparam>
public partial class DaprApplicationBus<TMessage, TMetadata, TState> : IMessageBus<TMessage, TMetadata, TState>
    where TMessage : BaseMessage
    where TMetadata : BaseMetadata
    where TState : MessageState<TMessage, TMetadata>, new()
{
    /// <summary>
    /// The dapr client.
    /// </summary>
    private readonly DaprClient _daprClient;

    /// <summary>
    /// The date time service.
    /// </summary>
    private readonly IDateTimeService _dateTimeService;

    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// The name.
    /// </summary>
    private readonly string _name;

    /// <summary>
    /// The topic suffix.
    /// </summary>
    private readonly string _topicSuffix;

    /// <summary>
    /// Initializes a new instance of the <see cref="DaprApplicationBus{TMessage, TMetadata, TState}" /> class.
    /// </summary>
    /// <param name="daprClient">The dapr client.</param>
    /// <param name="dateTimeService">The date time service.</param>
    /// <param name="name">The name.</param>
    /// <param name="topicSuffix">The topic suffix.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentNullException">Argument null.</exception>
    public DaprApplicationBus(
        DaprClient daprClient,
        IDateTimeService dateTimeService,
        string name,
        string topicSuffix,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(daprClient);
        ArgumentNullException.ThrowIfNull(dateTimeService);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(topicSuffix);
        ArgumentNullException.ThrowIfNull(logger);
        _daprClient = daprClient;
        _name = name;
        _logger = logger;
        _topicSuffix = topicSuffix;
        _dateTimeService = dateTimeService;
    }

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "Error while publishing on {BusName}/{TopicName} ({BusType}) the message {MessageName} Id={MessageId} CorrelationId={CorrelationId}.\nError : {ErrorMessage}\nMetadata :\n{Metadata}\nData:\n{Data}")]
    public partial void LogErrorWhileSendingMessage(
        Exception ex,
        string messageName,
        string messageId,
        string correlationId,
        string topicName,
        string busName,
        string busType,
        string errorMessage,
        string metadata,
        string data);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Information,
        Message = "Sent message : Name={MessageName}; Id='{MessageId}'; Correlation='{CorrelationId}' on {TopicName} of the {BusName} bus.")]
    public partial void LogMessageSent(string messageName, string messageId, string correlationId, string topicName, string busName);

    /// <inheritdoc/>
    public async Task PublishAsync(IEnvelope<TMessage, TMetadata> envelope, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(envelope);
        await PublishAsync(envelope.Message, envelope.Metadata, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task PublishAsync(TMessage message, TMetadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(metadata);
        TState state = new() { ReceivedDate = _dateTimeService.UtcNow, Message = message, Metadata = metadata };
        await PublishAsync(state, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task PublishAsync(TState envelope, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(envelope);
        ArgumentNullException.ThrowIfNull(envelope.Message);
        ArgumentNullException.ThrowIfNull(envelope.Metadata);
        string topicName = !string.IsNullOrEmpty(envelope.Message.AggregateName)
            ? envelope.Message.AggregateName.ToLowerInvariant() + _topicSuffix
            : throw new InvalidOperationException("Event aggregate name is not defined.");
        Dictionary<string, string> m = new(StringComparer.Ordinal)
                    {
                        { "ContentType", "application/json" },
                        { "Label", envelope.Metadata.Message.Name + ' ' + envelope.Message.AggregateId },
                        { "MessageName", envelope.Metadata.Message.Name },
                        { "MessageId", envelope.Metadata.Message.Id },
                        { "CorrelationId", envelope.Metadata.Context.CorrelationId },
                        { "SessionId", envelope.Metadata.Context.SessionId ?? envelope.Message.AggregateId },
                        { "PartitionKey", envelope.Message.AggregateId },
                    };
        try
        {
            await _daprClient.PublishEventAsync(
                _name,
                topicName,
                envelope,
                m,
                cancellationToken).ConfigureAwait(false);
            LogMessageSent(
                envelope.Metadata.Message.Name,
                envelope.Metadata.Message.Id,
                envelope.Metadata.Context.CorrelationId,
                topicName,
                _name);
        }
        catch (Exception ex)
        {
            LogErrorWhileSendingMessage(
                ex,
                envelope.Metadata.Message.Name,
                envelope.Metadata.Message.Id,
                envelope.Metadata.Context.CorrelationId,
                topicName,
                _name,
                GetType().Name,
                ex.FullMessage(),
                string.Join("\n", m.Select(p => $"{p.Key}={p.Value}")),
                JsonSerializer.Serialize(envelope));
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task PublishAsync(Application.MessageMetadatas.MessageState message, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);
        await PublishAsync(message.Message, message.Metadata, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task PublishAsync(object message, Application.MessageMetadatas.Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(metadata);
        string topicName = !string.IsNullOrEmpty(metadata.Message.Aggregate.Name)
            ? metadata.Message.Aggregate.Name.ToLowerInvariant() + _topicSuffix
            : throw new InvalidOperationException("Event aggregate name is not defined.");
        Dictionary<string, string> m = new(StringComparer.Ordinal)
                    {
                        { "ContentType", "application/json" },
                        { "Label", metadata.Message.Name + ' ' + metadata.Message.Aggregate.Name },
                        { "MessageName", metadata.Message.Name },
                        { "MessageId", metadata.Message.Id },
                        { "CorrelationId", metadata.Context.CorrelationId },
                        { "SessionId", metadata.Context.SessionId ?? metadata.Message.Aggregate.Name },
                        { "PartitionKey", metadata.Message.Aggregate.Id },
                    };
        Application.MessageMetadatas.MessageState state = Application.MessageMetadatas.MessageState.Create(message, metadata);
        try
        {
            await _daprClient.PublishEventAsync(
                _name,
                topicName,
                state,
                cancellationToken).ConfigureAwait(false);
            LogMessageSent(
                metadata.Message.Name,
                metadata.Message.Id,
                metadata.Context.CorrelationId,
                topicName,
                _name);
        }
        catch (Exception ex)
        {
            LogErrorWhileSendingMessage(
                ex,
                metadata.Message.Name,
                metadata.Message.Id,
                metadata.Context.CorrelationId,
                topicName,
                _name,
                GetType().Name,
                ex.FullMessage(),
                string.Join("\n", m.Select(p => $"{p.Key}={p.Value}")),
                JsonSerializer.Serialize(state));
            throw;
        }
    }
}