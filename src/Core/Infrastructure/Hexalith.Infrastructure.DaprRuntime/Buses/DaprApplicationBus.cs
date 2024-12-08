// <copyright file="DaprApplicationBus.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Buses;

using System.Text.Json;

using Dapr.Client;

using Hexalith.Application.Envelopes;
using Hexalith.Application.Metadatas;
using Hexalith.Extensions.Helpers;

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
    public static partial void LogErrorWhileSendingMessage(
        ILogger logger,
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
        Message = "Sent message : Name={MessageName}; AggregateGlobalId={AggregateGlobalId}; Id='{MessageId}'; Correlation='{CorrelationId}' on {TopicName} of the {BusName} bus.")]
    public static partial void LogMessageSent(ILogger logger, string messageName, string messageId, string correlationId, string aggregateGlobalId, string topicName, string busName);

    /// <inheritdoc/>
    public async Task PublishAsync(object message, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(metadata);

        // Determine the topic name based on the aggregate name
        string topicName = !string.IsNullOrEmpty(metadata.Message.Aggregate.Name)
            ? metadata.Message.Aggregate.Name.ToLowerInvariant() + _topicSuffix
            : throw new InvalidOperationException("Event aggregate name is not defined.");

        // Prepare metadata dictionary
        Dictionary<string, string> m = new(StringComparer.Ordinal)
        {
            { "ContentType", "application/json" },
            { "Time", dateTimeService.GetLocalNow().ToString("O") },
            { "Label", metadata.Message.Name + ' ' + metadata.Message.Aggregate.Id },
            { "MessageName", metadata.Message.Name },
            { "MessageId", metadata.Message.Id },
            { "CorrelationId", metadata.Context.CorrelationId },
            { "SessionId",  metadata.AggregateGlobalId },
            { "PartitionKey", metadata.AggregateGlobalId },
        };

        BusMessage state = BusMessage.Create(message, metadata);

        try
        {
            // Publish the event using Dapr client
            await daprClient.PublishEventAsync(
                _name,
                topicName,
                state,
                m,
                cancellationToken).ConfigureAwait(false);

            // Log successful message sending
            LogMessageSent(
                logger,
                metadata.Message.Name,
                metadata.Message.Id,
                metadata.Context.CorrelationId,
                metadata.AggregateGlobalId,
                topicName,
                _name);
        }
        catch (Exception ex)
        {
            // Log error details if message sending fails
            LogErrorWhileSendingMessage(
                logger,
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