// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprBus
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-05-2023
// ***********************************************************************
// <copyright file="DaprApplicationBus.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Buses;

using Dapr.Client;

using Hexalith.Application.Envelopes;
using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Domain.Messages;
using Hexalith.Extensions.Common;

using Microsoft.Extensions.Logging;

/// <summary>
/// This class is used to store events in a Dapr actor state.
/// </summary>
/// <typeparam name="TMessage">The type of the state.Message.</typeparam>
/// <typeparam name="TMetadata">The type of the state.Metadata.</typeparam>
/// <typeparam name="TState">The type of the t state.</typeparam>
public class DaprApplicationBus<TMessage, TMetadata, TState> : IMessageBus<TMessage, TMetadata, TState>
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
        {
            ArgumentNullException.ThrowIfNull(envelope);
            ArgumentNullException.ThrowIfNull(envelope.Message);
            ArgumentNullException.ThrowIfNull(envelope.Metadata);
            try
            {
                string topicName = !string.IsNullOrEmpty(envelope.Message.AggregateName) ? envelope.Message.AggregateName.ToLowerInvariant() : throw new Exception("Event aggregate name is not defined.");
                Dictionary<string, string> m = new(StringComparer.Ordinal)
                    {
                        { "MessageName", envelope.Metadata.Message.Name ?? string.Empty },
                        { "MessageId", envelope.Metadata.Message.Id },
                        { "CorrelationId", envelope.Metadata.Context.CorrelationId },
                        { "SessionId", envelope.Metadata.Context.SessionId ?? envelope.Message.AggregateId },
                        { "PartitionKey", envelope.Message.AggregateId },
                    };
                await _daprClient.PublishEventAsync(
                    _name,
                    topicName + _topicSuffix,
                    envelope,
                    m,
                    cancellationToken).ConfigureAwait(false);
                _logger.LogInformation(
                    "Sent message : Name={MessageName}; Id='{MessageId}'; Correlation='{CorrelationId}' on {TopicName} of the {BusName} bus.",
                    envelope.Metadata.Message.Name,
                    envelope.Metadata.Message.Id,
                    envelope.Metadata.Context.CorrelationId,
                    topicName + _topicSuffix,
                    _name);
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Error while publishing on {BusName}/{Topic} ({BusType}) the message {MessageName} Id={MessageId} CorrelationId={CorrelationId}.}",
                    _name,
                    envelope.Message.AggregateName + _topicSuffix,
                    GetType().Name,
                    envelope.Metadata.Message.Name,
                    envelope.Metadata.Message.Id,
                    envelope.Metadata.Context.CorrelationId);
                throw;
            }
        }
    }
}