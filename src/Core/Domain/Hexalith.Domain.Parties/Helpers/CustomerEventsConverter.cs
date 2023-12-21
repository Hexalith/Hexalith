// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 12-14-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-14-2023
// ***********************************************************************
// <copyright file="CustomerEventsConverter.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Helpers;

using System;
using System.Diagnostics.CodeAnalysis;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Domain.ValueObjets;

/// <summary>
/// Class CustomerEventsConverter.
/// </summary>
public static class CustomerEventsConverter
{
    /// <summary>
    /// Converts to customer.
    /// </summary>
    /// <param name="change">The event.</param>
    /// <param name="intercompanyDropship">if set to <c>true</c> [intercompany dropship].</param>
    /// <returns>Customer.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static Customer ToCustomer([NotNull] this CustomerInformationChanged change, bool intercompanyDropship)
    {
        ArgumentNullException.ThrowIfNull(change);
        return new Customer(
            change.PartitionId,
            change.CompanyId,
            change.OriginId,
            change.Id,
            change.Name,
            change.PartyType,
            new Contact(change.Contact),
            change.WarehouseId,
            change.CompanyId,
            change.GroupId,
            change.SalesCurrencyId,
            intercompanyDropship,
            change.Date);
    }

    /// <summary>
    /// Converts to change customer information.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <returns>ChangeCustomerInformation.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    public static CustomerInformationChanged ToCustomerInformationChanged([NotNull] this Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);
        return new CustomerInformationChanged(
            customer.PartitionId,
            customer.CompanyId,
            customer.OriginId,
            customer.Id,
            customer.Name,
            customer.PartyType,
            new Contact(customer.Contact),
            customer.WarehouseId,
            customer.CompanyId,
            customer.GroupId,
            customer.SalesCurrencyId,
            customer.Date);
    }

    /// <summary>
    /// Converts to register customer.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <returns>RegisterCustomer.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    public static CustomerRegistered ToCustomerRegistered([NotNull] this Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);
        return new CustomerRegistered(
            customer.PartitionId,
            customer.CompanyId,
            customer.OriginId,
            customer.Id,
            customer.Name,
            customer.PartyType,
            new Contact(customer.Contact),
            customer.WarehouseId,
            customer.CompanyId,
            customer.GroupId,
            customer.SalesCurrencyId,
            customer.Date);
    }
}