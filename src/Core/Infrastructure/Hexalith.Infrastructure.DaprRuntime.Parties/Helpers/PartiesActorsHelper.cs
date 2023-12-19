// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.Customer
// Author           : Jérôme Piquot
// Created          : 09-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-29-2023
// ***********************************************************************
// <copyright file="PartiesActorsHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Parties.Helpers;

using System.Diagnostics.CodeAnalysis;

using Dapr.Actors;
using Dapr.Actors.Client;
using Dapr.Actors.Runtime;

using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.Parties.Actors;

/// <summary>
/// Class PartiesHelper.
/// </summary>
public static class PartiesActorsHelper
{
    /// <summary>
    /// Adds the parties.
    /// </summary>
    /// <param name="actors">The actors.</param>
    /// <returns>ActorRegistrationCollection.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static ActorRegistrationCollection AddPartiesAggregates([NotNull] this ActorRegistrationCollection actors)
    {
        ArgumentNullException.ThrowIfNull(actors);
        actors.RegisterActor<CustomerAggregateActor>();
        return actors;
    }

    /// <summary>
    /// Adds the parties projection.
    /// </summary>
    /// <param name="actors">The actors.</param>
    /// <param name="prefix">The prefix.</param>
    /// <returns>ActorRegistrationCollection.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static ActorRegistrationCollection AddPartiesProjections([NotNull] this ActorRegistrationCollection actors, string prefix)
    {
        ArgumentNullException.ThrowIfNull(actors);
        actors.RegisterActor<KeyValueActor>(GetCustomerProjectionActorName(prefix));
        return actors;
    }

    /// <summary>
    /// Gets the customer projection actor.
    /// </summary>
    /// <param name="actorfactory">The actorfactory.</param>
    /// <param name="prefix">The prefix.</param>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <returns>IKeyValueActor.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static IKeyValueActor GetCustomerProjectionActor([NotNull] this IActorProxyFactory actorfactory, string prefix, string aggregateId)
    {
        ArgumentNullException.ThrowIfNull(actorfactory);

        return actorfactory.CreateActorProxy<IKeyValueActor>(new ActorId(aggregateId), GetCustomerProjectionActorName(prefix));
    }

    /// <summary>
    /// Gets the name of the customer projection actor.
    /// </summary>
    /// <param name="prefix">The prefix.</param>
    /// <returns>System.String.</returns>
    private static string GetCustomerProjectionActorName(string prefix) => prefix + "CustomerProjectiondActor";
}