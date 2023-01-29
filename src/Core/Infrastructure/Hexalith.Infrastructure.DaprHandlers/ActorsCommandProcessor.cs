// <copyright file="ActorsCommandProcessor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprHandlers;

using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Ardalis.GuardClauses;

using Dapr.Actors.Client;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Application.Abstractions.Metadatas;

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
        ArgumentNullException.ThrowIfNull(command);
        ArgumentException.ThrowIfNullOrEmpty(command.AggregateName);
        ArgumentException.ThrowIfNullOrEmpty(command.AggregateId);

        string actorName = GetActorName(command);
        string methodName = GetActorMethodName(command);

        ActorProxy actor = _actorProxy.Create(
            new Dapr.Actors.ActorId(command.AggregateId),
            actorName);

        IEnumerable<MethodInfo> methods = typeof(ActorProxy)
            .GetMethods()
            .Where(x =>
                x.Name == nameof(ActorProxy.InvokeMethodAsync) &&
                x.GetParameters().Length == 3 &&
                x.ContainsGenericParameters == true &&
                x.GetGenericArguments().Length == 1);
        MethodInfo? mi = methods.SingleOrDefault();
        if (mi == null)
        {
            if (methods.Count() == 0)
            {
                throw new InvalidOperationException($"The method '{nameof(ActorProxy.InvokeMethodAsync)}' not found on {nameof(ActorProxy)}.");
            }
            else
            {
                throw new InvalidOperationException($"Duplicate method '{nameof(ActorProxy.InvokeMethodAsync)}' found on {nameof(ActorProxy)}.");
            }
        }

        MethodInfo? generic = mi?.MakeGenericMethod(typeof(ActorCommandEnvelope));

        if (generic == null)
        {
            throw new InvalidOperationException($"Could not make the generic method '{nameof(ActorProxy)}.{nameof(ActorProxy.InvokeMethodAsync)}<{nameof(ActorCommandEnvelope)}>'.");
        }

        if (generic.Invoke(
            actor,
            new object[]
            {
                methodName,
                new ActorCommandEnvelope(command, metadata),
                cancellationToken,
            })
            is not Task task)
        {
            throw new InvalidOperationException($"The actor {actorName} method '{methodName}' should have a return value of type Task.");
        }

        try
        {
            await task;
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"Fail to call actor {actorName} method '{methodName}'.", e);
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