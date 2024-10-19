﻿// <copyright file="DaprApplicationBus.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Buses;

using System.Text.Json;

using Dapr.Client;

using Hexalith.Application.Envelopes;
using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Extensions.Helpers;
using Hexalith.PolymorphicSerialization;

using Microsoft.Extensions.Logging;

/// <summary>
/// Represents a Dapr-based implementation of the application message bus.
/// </summary>
public partial class DaprApplicationBus(
    DaprClient daprClient,
    TimeProvider dateTimeService,
    string name,
    string topicSuffix,
    ILogger logger) : IMessageBus
{
    /// <summary>
    /// The Dapr client used for publishing events.
    /// </summary>
    private readonly DaprClient _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));

    /// <summary>
    /// The time provider service.
    /// </summary>
    private readonly TimeProvider _dateTimeService = dateTimeService ?? throw new ArgumentNullException(nameof(dateTimeService));

    /// <summary>
    /// The logger used for logging operations and errors.
    /// </summary>
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// The name of the message bus.
    /// </summary>
    private readonly string _name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));

    /// <summary>
    /// The suffix appended to topic names.
    /// </summary>
    private readonly string _topicSuffix = !string.IsNullOrWhiteSpace(topicSuffix) ? topicSuffix : throw new ArgumentException("Topic suffix cannot be null or whitespace.", nameof(topicSuffix));

    /// <summary>
    /// Logs an error that occurred while sending a message.
    /// </summary>
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

    /// <summary>
    /// Logs information about a successfully sent message.
    /// </summary>
    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Information,
        Message = "Sent message : Name={MessageName}; Id='{MessageId}'; Correlation='{CorrelationId}' on {TopicName} of the {BusName} bus.")]
    public partial void LogMessageSent(string messageName, string messageId, string correlationId, string topicName, string busName);

    /// <inheritdoc/>
    public async Task PublishAsync(object message, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);
        await PublishAsync(new MessageState((PolymorphicRecordBase)message, metadata), cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task PublishAsync(MessageState message, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);

        // Determine the topic name based on the aggregate name
        string topicName = !string.IsNullOrEmpty(message.Metadata.Message.Aggregate.Name)
            ? message.Metadata.Message.Aggregate.Name.ToLowerInvariant() + _topicSuffix
            : throw new InvalidOperationException("Event aggregate name is not defined.");

        // Prepare metadata dictionary
        Dictionary<string, string> m = new(StringComparer.Ordinal)
        {
            { "ContentType", "application/json" },
            { "Label", message.Metadata.Message.Name + ' ' + message.Metadata.Message.Aggregate.Name },
            { "MessageName", message.Metadata.Message.Name },
            { "MessageId", message.Metadata.Message.Id },
            { "CorrelationId", message.Metadata.Context.CorrelationId },
            { "SessionId", message.Metadata.Context.SessionId ?? message.Metadata.Message.Aggregate.Name },
            { "PartitionKey", message.Metadata.PartitionKey },
        };

        try
        {
            // Publish the event using Dapr client
            await _daprClient.PublishEventAsync(
                _name,
                topicName,
                message,
                m,
                cancellationToken).ConfigureAwait(false);

            // Log successful message sending
            LogMessageSent(
                message.Metadata.Message.Name,
                message.Metadata.Message.Id,
                message.Metadata.Context.CorrelationId,
                topicName,
                _name);
        }
        catch (Exception ex)
        {
            // Log error details if message sending fails
            LogErrorWhileSendingMessage(
                ex,
                message.Metadata.Message.Name,
                message.Metadata.Message.Id,
                message.Metadata.Context.CorrelationId,
                topicName,
                _name,
                GetType().Name,
                ex.FullMessage(),
                string.Join("\n", m.Select(p => $"{p.Key}={p.Value}")),
                JsonSerializer.Serialize(message, PolymorphicHelper.DefaultJsonSerializerOptions));
            throw;
        }
    }
}