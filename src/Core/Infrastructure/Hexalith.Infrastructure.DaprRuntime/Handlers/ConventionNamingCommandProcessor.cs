// <copyright file="ConventionNamingCommandProcessor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Handlers;

using Ardalis.GuardClauses;

using Dapr.Actors.Client;

using Hexalith.Application.Commands;

/// <summary>
/// Convention naming command processor.
/// The processor will call the method named 'Do+commandTypeName+Async' on the actor named 'aggregateName+AggregateActor'.
/// The command SubmitNewOrder will call the method DoSubmitNewOrderAsync on the actor named SalesOrderAggregateActor.
/// Implements the <see cref="ActorsCommandProcessor" />.
/// </summary>
/// <seealso cref="ActorsCommandProcessor" />
public class ConventionNamingCommandProcessor : ActorsCommandProcessor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConventionNamingCommandProcessor" /> class.
    /// </summary>
    /// <param name="actorProxy">The actor proxy.</param>
    public ConventionNamingCommandProcessor(IActorProxyFactory actorProxy)
        : base(actorProxy)
    {
    }

    /// <inheritdoc/>
    protected override string GetActorMethodName(ICommand command)
    {
        _ = Guard.Against.Null(command);
        return "Do" + command.GetType().Name + "Async";
    }

    /// <inheritdoc/>
    protected override string GetActorName(ICommand command)
    {
        _ = Guard.Against.Null(command);
        return command.AggregateName + "AggregateActor";
    }
}