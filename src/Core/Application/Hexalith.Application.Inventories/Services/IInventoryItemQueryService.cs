// ***********************************************************************
// Assembly         : Hexalith.Application.Parties
// Author           : Jérôme Piquot
// Created          : 08-29-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-29-2023
// ***********************************************************************
// <copyright file="IInventoryItemQueryService.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Inventories.Services;

using Hexalith.Domain.Events;

/// <summary>
/// Interface ICustomerQueryService.
/// </summary>
public interface IInventoryItemQueryService
{
    /// <summary>
    /// Creates the information changed event asynchronous.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;InventoryItemInformationChanged&gt;&gt;.</returns>
    Task<InventoryItemInformationChanged?> CreateInformationChangedEventAsync(string aggregateId, CancellationToken cancellationToken);

    /// <summary>
    /// Exists the asynchronous.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Boolean&gt;.</returns>
    Task<bool> ExistAsync(string aggregateId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the item name asynchronous.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="none">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;System.String&gt;&gt;.</returns>
    Task<string?> GetItemNameAsync(string aggregateId, CancellationToken none);

    /// <summary>
    /// Determines whether [has changes asynchronous] [the specified change].
    /// </summary>
    /// <param name="change">The change.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Boolean&gt;.</returns>
    Task<bool> HasChangesAsync(InventoryItemInformationChanged change, CancellationToken cancellationToken);
}