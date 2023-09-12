// ***********************************************************************
// Assembly         : Bspk.InventoryOnHands
// Author           : Jérôme Piquot
// Created          : 08-30-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-12-2023
// ***********************************************************************
// <copyright file="IAggregateExternalReferenceAggregateActor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Actors;

using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;

using Hexalith.Domain.ValueObjets;

/// <summary>
/// Interface IAggregateExternalReferenceAggregateActor
/// Extends the <see cref="IActor" />.
/// </summary>
/// <seealso cref="IActor" />
public interface IAggregateExternalReferenceAggregateActor : IActor
{
    /// <summary>
    /// Gets the external references.
    /// </summary>
    /// <returns>Task&lt;IEnumerable&lt;ExternalReference&gt;&gt;.</returns>
    Task<IEnumerable<ExternalReference>> GetExternalReferencesAsync();

    /// <summary>
    /// Gets the system reference.
    /// </summary>
    /// <param name="systemId">The system identifier.</param>
    /// <returns>Task&lt;System.Nullable&lt;System.String&gt;&gt;.</returns>
    Task<string?> GetSystemReferenceAsync(string systemId);
}