// <copyright file="DomainActorCommandProcessor.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Handlers;

using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Client;

using Hexalith.Application;
using Hexalith.Application.Commands;
using Hexalith.Application.MessageMetadatas;
using Hexalith.Infrastructure.DaprRuntime.Actors;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class AggregateActorCommandProcessor.
/// Implements the <see cref="ICommandProcessor" />.
/// </summary>
/// <seealso cref="ICommandProcessor" />
public partial class DomainActorCommandProcessor : IDomainCommandProcessor
{
    /// <summary>
    /// The actor proxy.
    /// </summary>
    private readonly IActorProxyFactory _actorProxy;

    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainActorCommandProcessor"/> class.
    /// </summary>
    /// <param name="actorProxy">The actor proxy.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public DomainActorCommandProcessor(IActorProxyFactory actorProxy, ILogger<DomainActorCommandProcessor> logger)
    {
        ArgumentNullException.ThrowIfNull(actorProxy);
        ArgumentNullException.ThrowIfNull(logger);
        _actorProxy = actorProxy;
        _logger = logger;
    }

    /// <summary>
    /// Logs the sending commands to actor.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="commandName">Name of the command.</param>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="actorName">Name of the actor.</param>
    [LoggerMessage(
                EventId = 2,
                Level = LogLevel.Information,
                Message = "Sending command {CommandName} to aSystem.NotSupportedException: 'Runtime type 'Manhole.Commands.Factories.AddFactory' is not supported by polymorphic type 'Hexalith.PolymorphicSerialization.PolymorphicRecoctor {ActorName} for aggregate {AggregateId}.")]
    public static partial void LogSendingCommandToActor(ILogger logger, string commandName, string aggregateId, string actorName);

    /// <inheritdoc/>
    public async Task SubmitAsync(object command, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(metadata);
        string actorName = DomainAggregateActorBase.GetAggregateActorName(metadata.Message.Aggregate.Name);
        try
        {
            LogSendingCommandToActor(_logger, metadata.Message.Name, metadata.Message.Aggregate.Id, actorName);
            IDomainAggregateActor actor = _actorProxy.CreateActorProxy<IDomainAggregateActor>(new ActorId(metadata.Message.Aggregate.Id), actorName);
            ActorMessageEnvelope envelope = ActorMessageEnvelope.Create(command, metadata);
            string json = JsonSerializer.Serialize(envelope, ApplicationConstants.DefaultJsonSerializerOptions);
            await actor.SubmitCommandAsJsonAsync(json).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"Fail to call actor {actorName} method '{nameof(IDomainAggregateActor.SubmitCommandAsync)}'.", e);
        }
    }
}