// ***********************************************************************
// Assembly         : Hexalith.Application.Parties
// Author           : Jérôme Piquot
// Created          : 08-29-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-29-2023
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
using Hexalith.Domain.Aggregates;

/// <summary>
/// Interface ICustomerQueryService.
/// </summary>
public interface ICustomerQueryService
{
    /// <summary>
    /// Exist the asynchronous.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="customerId">The customer identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Boolean&gt;.</returns>
    Task<bool> ExistAsync(string companyId, string customerId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the asynchronous.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="customerId">The customer identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;Customer&gt;.</returns>
    Task<Customer> GetAsync(string companyId, string customerId, CancellationToken cancellationToken);

    /// <summary>
    /// Determines whether [has changes asynchronous] [the specified change].
    /// </summary>
    /// <param name="change">The change.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Boolean&gt;.</returns>
    Task<bool> HasChangesAsync(ChangeCustomerInformation change, CancellationToken cancellationToken);
}