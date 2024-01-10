// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.Customer
// Author           : Jérôme Piquot
// Created          : 09-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-29-2023
// ***********************************************************************
// <copyright file="SalesActorsHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Sales.Helpers;

using System.Diagnostics.CodeAnalysis;

using Dapr.Actors.Runtime;

using Hexalith.Domain.Aggregates;
using Hexalith.Infrastructure.DaprRuntime.Sales.Actors;

/// <summary>
/// Class SalesHelper.
/// </summary>
public static class SalesActorsHelper
{
    /// <summary>
    /// Adds the parties.
    /// </summary>
    /// <param name="actors">The actors.</param>
    /// <returns>ActorRegistrationCollection.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static ActorRegistrationCollection AddSalesAggregates([NotNull] this ActorRegistrationCollection actors)
    {
        ArgumentNullException.ThrowIfNull(actors);
        actors.RegisterActor<AggregateActor>(AggregateActor.GetAggregateActorName(SalesInvoice.GetAggregateName()));
        return actors;
    }

    /// <summary>
    /// Adds the parties projections.
    /// </summary>
    /// <param name="actors">The actors.</param>
    /// <param name="applicationName">Name of the application.</param>
    /// <returns>ActorRegistrationCollection.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static ActorRegistrationCollection AddSalesProjections([NotNull] this ActorRegistrationCollection actors, string applicationName)
    {
        ArgumentNullException.ThrowIfNull(actors);

        // actors.RegisterProjectionActor<CustomerRegistered>(applicationName);
        return actors;
    }
}