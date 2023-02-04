// <copyright file="DaprApplicationBus.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprBus;

using System.Threading;
using System.Threading.Tasks;

using Dapr.Client;

using Hexalith.Application.Abstractions.Envelopes;
using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Application.Abstractions.States;
using Hexalith.Domain.Abstractions.Messages;
using Hexalith.Extensions.Common;

using Microsoft.Extensions.Logging;

/// <summary>
/// This class is used to store events in a Dapr actor state.
/// </summary>
/// <typeparam name="TMessage">The type of the message.</typeparam>
/// <typeparam name="TMetadata">The type of the metadata.</typeparam>
public class DaprApplicationBus<TMessage, TMetadata> : IMessageBus<TMessage, TMetadata>
    where TMessage : BaseMessage
    where TMetadata : Metadata
{
    /// <summary>
    /// The dapr client.
    /// </summary>
    private readonly DaprClient _daprClient;

    private readonly IDateTimeService _dateTimeService;

    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// The name.
    /// </summary>
    private readonly string _name;

    private readonly string _topicSuffix;

    /// <summary>
    /// Initializes a new instance of the <see cref="DaprApplicationBus{TMessage, TMetadata}"/> class.
    /// </summary>
    /// <param name="daprClient">The dapr client.</param>
    /// <param name="dateTimeService">The date time service.</param>
    /// <param name="name">The name.</param>
    /// <param name="topicSuffix">The topic suffix.</param>
    /// <param name="logger">The logger.</param>
    public DaprApplicationBus(
        DaprClient daprClient,
        IDateTimeService dateTimeService,
        string name,
        string topicSuffix,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(daprClient);
        ArgumentNullException.ThrowIfNull(dateTimeService);
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(topicSuffix);
        ArgumentNullException.ThrowIfNull(logger);
        _daprClient = daprClient;
        _name = name;
        _logger = logger;
        _topicSuffix = topicSuffix;
        _dateTimeService = dateTimeService;
    }

    /// <inheritdoc/>
    public Task PublishAsync(IEnvelope<TMessage, TMetadata> envelope, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(envelope);
        return PublishAsync(envelope.Message, envelope.Metadata, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task PublishAsync(TMessage message, TMetadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(metadata);
        _logger.LogInformation(
            "Sending event : Name={MessageName}; Id='{MessageId}'; Correlation='{CorrelationId}'.",
            metadata.Message.Name,
            metadata.Message.Id,
            metadata.Context.CorrelationId);
        try
        {
            string topicName = !string.IsNullOrEmpty(message.AggregateName) ? message.AggregateName : throw new Exception("Event aggregate name is not defined.");
            await _daprClient.PublishEventAsync(
                _name,
                topicName + _topicSuffix,
                new EventState(_dateTimeService.UtcNow, message, metadata),
                new Dictionary<string, string>(StringComparer.Ordinal)
                {
                { "MessageName", metadata.Message.Name ?? string.Empty },
                { "MessageId", metadata.Message.Id },
                { "CorrelationId", metadata.Context.CorrelationId },
                { "SessionId", metadata.Context.SessionId ?? message.AggregateId },
                { "PartitionKey", message.AggregateId },
                },
                cancellationToken).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.LogError(
                e,
                "Error while publishing on {BusName}/{Topic} ({BusType}) the message {MessageName} Id={MessageId} CorrelationId={CorrelationId}.}",
                _name,
                message.AggregateName + "-event",
                GetType().Name,
                metadata.Message.Name,
                metadata.Message.Id,
                metadata.Context.CorrelationId);
            throw;
        }
    }
}