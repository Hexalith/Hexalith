// <copyright file="ConventionCommandDispatcher.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprDispatcher;

using Dapr.Actors.Client;

using Hexalith.Application.Abstractions.Commands;

/// <summary>
/// Convention naming command dispatcher.
/// The dispatcher will call the method named 'Do+commandTypeName' on the actor named 'aggragateName+CommandActor'.
/// The command SubmitNewOrder will call the method DoSubmitNewOrder on the actor named SalesOrderCommandActor.
/// Implements the <see cref="Hexalith.Infrastructure.DaprDispatcher.ActorsCommandDispatcher" />.
/// </summary>
/// <seealso cref="Hexalith.Infrastructure.DaprDispatcher.ActorsCommandDispatcher" />
public class ConventionNamingCommandDispatcher : ActorsCommandDispatcher
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConventionNamingCommandDispatcher" /> class.
    /// </summary>
    /// <param name="actorProxy">The actor proxy.</param>
    public ConventionNamingCommandDispatcher(IActorProxyFactory actorProxy)
        : base(actorProxy)
    {
    }

    /// <inheritdoc/>
    protected override string GetActorMethodName(ICommand command)
    {
        return "Do" + command.GetType().Name;
    }

    /// <inheritdoc/>
    protected override string GetActorName(ICommand command)
    {
        return command.AggregateName + "CommandActor";
    }
}
