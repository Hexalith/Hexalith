// <copyright file="DomainActorCommandProcessor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Handlers;

using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors.Client;

using Hexalith.Application.Commands;
using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.PolymorphicSerialization;

using Microsoft.Extensions.Logging;

/// <summary>
/// Processes domain commands by routing them to appropriate Dapr actors.
/// This class handles the submission of commands to domain aggregates implemented as actors,
/// supporting both standard and polymorphic serialization of commands.
/// </summary>
/// <remarks>
/// The processor creates actor proxies for each aggregate and handles the serialization
/// of commands before sending them to the actors. It supports two modes of operation:
/// 1. Polymorphic serialization for complex type hierarchies
/// 2. Standard serialization for simple command types.
/// </remarks>
/// <seealso cref="IDomainCommandProcessor" />
public partial class DomainActorCommandProcessor : IDomainCommandProcessor
{
    /// <summary>
    /// Indicates whether the actor can handle polymorphic serialization. If false, commands are serialized before being sent as JSON text.
    /// </summary>
    private readonly bool _actorPolymorphicSerialization;

    /// <summary>
    /// The actor proxy factory used to create actor proxies for command processing.
    /// </summary>
    private readonly IActorProxyFactory _actorProxy;

    /// <summary>
    /// The logger instance for recording processing activities and errors.
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainActorCommandProcessor"/> class.
    /// </summary>
    /// <param name="actorProxy">The factory for creating actor proxies.</param>
    /// <param name="actorPolymorphicSerialization">Specifies whether the actor can handle polymorphic serialization, else serialize before and send the command as a json text.</param>
    /// <param name="logger">The logger for recording processing activities.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when actorProxy or logger is null.</exception>
    public DomainActorCommandProcessor(IActorProxyFactory actorProxy, bool actorPolymorphicSerialization, ILogger<DomainActorCommandProcessor> logger)
    {
        ArgumentNullException.ThrowIfNull(actorProxy);
        ArgumentNullException.ThrowIfNull(logger);
        _actorProxy = actorProxy;
        _logger = logger;
        _actorPolymorphicSerialization = actorPolymorphicSerialization;
        if (actorPolymorphicSerialization)
        {
            // Remove this exception when custom JSON serialization will be supported by DAPR actors.
            throw new NotSupportedException("Polymorphic serialization by DAPR actors is not supported yet.");
        }
    }

    /// <summary>
    /// Logs information about commands being sent to actors.
    /// </summary>
    /// <param name="logger">The logger instance to use.</param>
    /// <param name="commandName">The name of the command being sent.</param>
    /// <param name="aggregateGlobalId">The global identifier of the target aggregate.</param>
    /// <param name="aggregateName">The name of the aggregate.</param>
    [LoggerMessage(
                EventId = 2,
                Level = LogLevel.Information,
                Message = "Sending command {CommandName} to actor for aggregate {AggregateName} with global Id {AggregateGlobalId}.")]
    public static partial void LogSendingCommandToActor(ILogger logger, string commandName, string aggregateGlobalId, string aggregateName);

    /// <summary>
    /// Submits a command to be processed by the appropriate domain aggregate actor.
    /// </summary>
    /// <param name="command">The command to be processed.</param>
    /// <param name="metadata">The metadata associated with the command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown when command or metadata is null.</exception>
    /// <exception cref="System.InvalidOperationException">Thrown when the actor call fails.</exception>
    /// <remarks>
    /// The method handles two scenarios:
    /// 1. Commands that require polymorphic serialization (when enabled and the command is polymorphic).
    /// 2. Standard commands using regular JSON serialization.
    /// </remarks>
    public async Task SubmitAsync(object command, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(metadata);
        try
        {
            LogSendingCommandToActor(_logger, metadata.Message.Name, metadata.AggregateGlobalId, metadata.Message.Aggregate.Name);
            if (_actorPolymorphicSerialization && command is PolymorphicRecordBase polymorphicCommand)
            {
                await _actorProxy
                    .ToDomainAggregateActor(metadata)
                    .SubmitCommandAsStateAsync(new MessageState(polymorphicCommand, metadata))
                    .ConfigureAwait(false);
            }
            else
            {
                ActorMessageEnvelope envelope = ActorMessageEnvelope.Create(command, metadata);
                string json = JsonSerializer.Serialize(envelope, PolymorphicHelper.DefaultJsonSerializerOptions);
                await _actorProxy
                    .ToDomainAggregateActor(metadata)
                    .SubmitCommandAsJsonAsync(json)
                    .ConfigureAwait(false);
            }
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"Fail to call actor for aggregate {metadata.AggregateGlobalId} method '{nameof(IDomainAggregateActor.SubmitCommandAsync)}'.", e);
        }
    }
}