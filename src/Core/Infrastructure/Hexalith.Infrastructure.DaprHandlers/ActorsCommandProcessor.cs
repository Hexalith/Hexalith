// <copyright file="ActorsCommandProcessor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprHandlers;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Ardalis.GuardClauses;

using Dapr.Actors;
using Dapr.Actors.Client;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.DaprHandlers.Helpers;

/// <summary>
/// Class ActorsCommandProcessor.
/// Implements the <see cref="ICommandProcessor" />.
/// </summary>
/// <seealso cref="ICommandProcessor" />
public abstract class ActorsCommandProcessor : ICommandProcessor
{
    /// <summary>
    /// The actor proxy.
    /// </summary>
    private readonly IActorProxyFactory _actorProxy;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActorsCommandProcessor" /> class.
    /// </summary>
    /// <param name="actorProxy">The actor proxy.</param>
    public ActorsCommandProcessor(IActorProxyFactory actorProxy)
    {
        _actorProxy = Guard.Against.Null(actorProxy);
    }

    /// <inheritdoc/>
    public async Task SubmitAsync(BaseCommand command, Metadata metadata, CancellationToken cancellationToken)
    {
        await SubmitAsync(command.IntoArray(), metadata, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SubmitAsync(IEnumerable<BaseCommand> commands, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(commands);
        if (!commands.Any())
        {
            throw new InvalidOperationException("The list of commands is empty.");
        }

        BaseCommand command = commands.First();

        string actorName = GetActorName(command);
        List<Metadata> metadatas = new() { metadata };
        if (commands.Count() > 1)
        {
            metadatas
                .AddRange(commands
                    .Skip(1)
                    .Select(p => Metadata.CreateNew(p, metadata)));
        }

        ActorCommandEnvelope envelope = new(commands.ToArray(), metadatas.ToArray());

        try
        {
            ICommandProcessorActor actor = _actorProxy.CreateActorProxy<ICommandProcessorActor>(command.AggregateId.ToActorId(), actorName);
            await actor.DoAsync(envelope);
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"Fail to call actor {actorName} method '{nameof(ICommandProcessorActor.DoAsync)}'.", e);
        }
    }

    /// <summary>
    /// Gets the name of the actor method.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns>string.</returns>
    protected abstract string GetActorMethodName(ICommand command);

    /// <summary>
    /// Gets the name of the actor.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns>string.</returns>
    protected abstract string GetActorName(ICommand command);
}