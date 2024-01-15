// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.ExternalSystems
// Author           : Jérôme Piquot
// Created          : 09-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-12-2023
// ***********************************************************************
// <copyright file="ExternalSystemsAggregatesHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Aggregates.Helpers;

using System.Diagnostics.CodeAnalysis;

using Dapr.Actors.Runtime;

using Hexalith.Domain.Aggregates;
using Hexalith.Infrastructure.DaprRuntime.Sales.Actors;

/// <summary>
/// Class ExternalSystemsHelper.
/// </summary>
public static class ExternalSystemsAggregatesHelper
{
    /// <summary>
    /// Adds the external systems.
    /// </summary>
    /// <param name="actors">The actors.</param>
    /// <returns>ActorRegistrationCollection.</returns>
    public static ActorRegistrationCollection AddExternalSystemsAggregates([NotNull] this ActorRegistrationCollection actors)
    {
        ArgumentNullException.ThrowIfNull(actors);
        actors.RegisterActor<AggregateActor>(AggregateActor.GetAggregateActorName(ExternalSystemReference.GetAggregateName()));
        return actors;
    }
}