// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.Parties
// Author           : Jérôme Piquot
// Created          : 08-30-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-30-2023
// ***********************************************************************
// <copyright file="ICustomerAggregateActor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.DaprRuntime.Parties.Actors;

using System.Threading.Tasks;

using Dapr.Actors;

using Hexalith.Application.Parties.Commands;
using Hexalith.Domain.Events;

/// <summary>
/// Interface ICustomerAggregateActor
/// Extends the <see cref="IActor" />.
/// </summary>
/// <seealso cref="IActor" />
public interface ICustomerAggregateActor : IActor
{
    /// <summary>
    /// Creates the information changed event asynchronous.
    /// </summary>
    /// <returns>Task&lt;CustomerInformationChanged&gt;.</returns>
    Task<CustomerInformationChanged?> CreateInformationChangedEventAsync();

    /// <summary>
    /// Exists the asynchronous.
    /// </summary>
    /// <returns>Task&lt;System.Boolean&gt;.</returns>
    Task<bool> ExistAsync();

    /// <summary>
    /// Determines whether the specified command would change the customer information.
    /// </summary>
    /// <param name="change">The change.</param>
    /// <returns>Task&lt;System.Boolean&gt;.</returns>
    Task<bool> HasChangesAsync(ChangeCustomerInformation change);

    /// <summary>
    /// Determines whether [is intercompany direct delivery asynchronous].
    /// </summary>
    /// <returns>Task&lt;System.Boolean&gt;.</returns>
    Task<bool> IsIntercompanyDirectDeliveryAsync();
}