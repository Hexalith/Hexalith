// <copyright file="ConventionNamingCommandProcessor.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Handlers;

using Dapr.Actors.Client;

using Hexalith.Application.Commands;

using Microsoft.Extensions.Logging;

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
[Obsolete("Use AggregateActorCommandProcessor instead.", true)]
public class ConventionNamingCommandProcessor(IActorProxyFactory actorProxy, ILogger<ConventionNamingCommandProcessor> logger) : ActorsCommandProcessor(actorProxy, logger), IConventionNamingCommandProcessor
{
    /// <inheritdoc/>
    protected override string GetActorMethodName(ICommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        return "Do" + command.GetType().Name + "Async";
    }

    /// <inheritdoc/>
    protected override string GetActorName(ICommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        return command.AggregateName + "AggregateActor";
    }
}