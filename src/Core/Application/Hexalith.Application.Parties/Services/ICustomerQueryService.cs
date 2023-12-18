// ***********************************************************************
// Assembly         : Hexalith.Application.Parties
// Author           : Jérôme Piquot
// Created          : 08-29-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-12-2023
// ***********************************************************************
// <copyright file="ICustomerQueryService.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Parties.Services;

using Hexalith.Application.Parties.Commands;
using Hexalith.Domain.Events;

/// <summary>
/// Interface ICustomerQueryService.
/// </summary>
public interface ICustomerQueryService
{
    /// <summary>
    /// Creates the information changed event.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;CustomerInformationChanged&gt;.</returns>
    Task<CustomerInformationChanged?> CreateInformationChangedEventAsync(string aggregateId, CancellationToken cancellationToken);

    /// <summary>
    /// Exists the asynchronous.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Boolean&gt;.</returns>
    Task<bool> ExistAsync(string aggregateId, CancellationToken cancellationToken);

    /// <summary>
    /// Determines whether [has changes asynchronous] [the specified change].
    /// </summary>
    /// <param name="change">The change.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Boolean&gt;.</returns>
    Task<bool> HasChangesAsync(ChangeCustomerInformation change, CancellationToken cancellationToken);

    /// <summary>
    /// Determines whether [is intercompany direct delivery asynchronous] [the specified aggregate identifier].
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Boolean&gt;.</returns>
    Task<bool> IsIntercompanyDirectDeliveryAsync(string aggregateId, CancellationToken cancellationToken);
}