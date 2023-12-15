// ***********************************************************************
// Assembly         : Hexalith.Application.Parties
// Author           : Jérôme Piquot
// Created          : 12-10-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-10-2023
// ***********************************************************************
// <copyright file="CustomerCommandsConverter.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Parties.Helpers;

using System;
using System.Diagnostics.CodeAnalysis;

using Hexalith.Application.Parties.Commands;
using Hexalith.Domain.Aggregates;

/// <summary>
/// Class CustomerCommandsConverter.
/// </summary>
public static class CustomerCommandsConverter
{
    /// <summary>
    /// Converts to change customer information.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <returns>ChangeCustomerInformation.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static ChangeCustomerInformation ToChangeCustomerInformation([NotNull] this Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);
        return new ChangeCustomerInformation(
            customer.PartitionId,
            customer.CompanyId,
            customer.OriginId,
            customer.Id,
            customer.Name,
            customer.PartyType,
            customer.Contact,
            customer.WarehouseId,
            customer.CommissionSalesGroupId,
            customer.GroupId,
            customer.SalesCurrencyId,
            customer.Date);
    }

    /// <summary>
    /// Converts to customer.
    /// </summary>
    /// <param name="register">The event.</param>
    /// <param name="intercompanyDropship">if set to <c>true</c> [intercompany dropship].</param>
    /// <returns>Customer.</returns>
    public static Customer ToCustomer([NotNull] this RegisterCustomer register, bool intercompanyDropship)
    {
        ArgumentNullException.ThrowIfNull(register);
        return new Customer(
            register.PartitionId,
            register.CompanyId,
            register.OriginId,
            register.Id,
            register.Name,
            register.PartyType,
            register.Contact,
            register.WarehouseId,
            register.CompanyId,
            register.GroupId,
            register.SalesCurrencyId,
            intercompanyDropship,
            register.Date);
    }

    /// <summary>
    /// Converts to customer.
    /// </summary>
    /// <param name="change">The event.</param>
    /// <param name="intercompanyDropship">if set to <c>true</c> [intercompany dropship].</param>
    /// <returns>Customer.</returns>
    public static Customer ToCustomer([NotNull] this ChangeCustomerInformation change, bool intercompanyDropship)
    {
        ArgumentNullException.ThrowIfNull(change);
        return new Customer(
            change.PartitionId,
            change.CompanyId,
            change.OriginId,
            change.Id,
            change.Name,
            change.PartyType,
            change.Contact,
            change.WarehouseId,
            change.CompanyId,
            change.GroupId,
            change.SalesCurrencyId,
            intercompanyDropship,
            change.Date);
    }

    /// <summary>
    /// Converts to register customer.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <returns>RegisterCustomer.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static RegisterCustomer ToRegisterCustomer([NotNull] this Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);
        return new RegisterCustomer(
            customer.PartitionId,
            customer.CompanyId,
            customer.OriginId,
            customer.Id,
            customer.Name,
            customer.PartyType,
            customer.Contact,
            customer.WarehouseId,
            customer.CompanyId,
            customer.GroupId,
            customer.SalesCurrencyId,
            customer.Date);
    }
}