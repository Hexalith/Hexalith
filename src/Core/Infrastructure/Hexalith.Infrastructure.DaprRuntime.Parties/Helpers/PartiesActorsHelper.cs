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

using Dapr.Actors.Runtime;

using Hexalith.Domain.Aggregates;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Sales.Actors;

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
        actors.RegisterActor<AggregateActor>(AggregateActor.GetAggregateActorName(Customer.GetAggregateName()));
        return actors;
    }

    /// <summary>
    /// Adds the parties projections.
    /// </summary>
    /// <param name="actors">The actors.</param>
    /// <param name="applicationId">Name of the application.</param>
    /// <returns>ActorRegistrationCollection.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static ActorRegistrationCollection AddPartiesProjections([NotNull] this ActorRegistrationCollection actors, string applicationId)
    {
        ArgumentNullException.ThrowIfNull(actors);
        actors.RegisterProjectionActor<Customer>(applicationId);
        return actors;
    }
}