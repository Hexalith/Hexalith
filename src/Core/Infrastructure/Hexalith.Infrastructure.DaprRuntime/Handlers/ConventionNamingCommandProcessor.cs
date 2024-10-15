// <copyright file="ConventionNamingCommandProcessor.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Handlers;

/// <summary>
/// Convention naming command processor.
/// The processor will call the method named 'Do+commandTypeName+Async' on the actor named 'aggregateName+AggregateActor'.
/// The command SubmitNewOrder will call the method DoSubmitNewOrderAsync on the actor named SalesOrderAggregateActor.
/// Implements the <see cref="ActorsCommandProcessor" />.
/// </summary>
/// <seealso cref="ActorsCommandProcessor" />
/// <remarks>
/// Initializes a new instance of the <see cref="ConventionNamingCommandProcessor"/> class.
/// </remarks>
/// <param name="actorProxy">The actor proxy.</param>
/// <param name="logger">The logger.</param>
// [Obsolete("Use AggregateActorCommandProcessor instead.", true)]
// public class ConventionNamingCommandProcessor(IActorProxyFactory actorProxy, ILogger<ConventionNamingCommandProcessor> logger) : ActorsCommandProcessor(actorProxy, logger), IConventionNamingCommandProcessor
// {
//    /// <inheritdoc/>
//    protected override string GetActorMethodName(object command)
//    {
//        ArgumentNullException.ThrowIfNull(command);
//        return "Do" + command.GetType().Name + "Async";
//    }

// /// <inheritdoc/>
//    protected override string GetActorName(object command)
//    {
//        ArgumentNullException.ThrowIfNull(command);
//        return command.AggregateName + "AggregateActor";
//    }
// }