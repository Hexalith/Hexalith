// <copyright file="CommandDispatcher.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprDispatcher;

using Ardalis.GuardClauses;

using Dapr.Actors.Client;

using Hexalith.Application.Abstractions.Commands;

using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Class ActorsCommandDispatcher.
/// Implements the <see cref="ICommandDispatcher" />.
/// </summary>
/// <seealso cref="ICommandDispatcher" />
public abstract class ActorsCommandDispatcher : ICommandDispatcher
{
    /// <summary>
    /// The actor proxy.
    /// </summary>
    private readonly IActorProxyFactory _actorProxy;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActorsCommandDispatcher" /> class.
    /// </summary>
    /// <param name="actorProxy">The actor proxy.</param>
    public ActorsCommandDispatcher(IActorProxyFactory actorProxy)
    {
        _actorProxy = Guard.Against.Null(actorProxy);
    }

    /// <inheritdoc/>
    public async Task DoAsync(ICommand command, CancellationToken cancellationToken)
    {
        _ = Guard.Against.Null(command);
        _ = Guard.Against.NullOrWhiteSpace(command.AggregateName);
        _ = Guard.Against.NullOrWhiteSpace(command.AggregateId);
        string actorName = GetActorName(command);
        string methodName = GetActorMethodName(command);
        ActorProxy actor = _actorProxy.Create(
            new Dapr.Actors.ActorId(command.AggregateId),
            actorName);
        MethodInfo? mi = actor.GetType().GetMethod(nameof(ActorProxy.InvokeMethodAsync));
        if (mi == null)
        {
            throw new InvalidOperationException($"The method '{nameof(ActorProxy.InvokeMethodAsync)}' not found on {nameof(ActorProxy)}.");
        }

        Type commandType = command.GetType();
        MethodInfo generic = mi.MakeGenericMethod(commandType);
        if (generic.Invoke(
            actor,
            new object[] { methodName, command, cancellationToken }) is not Task task)
        {
            throw new InvalidOperationException($"The actor {actorName} method '{methodName}' should have a return value of type Task.");
        }

        await task;
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
