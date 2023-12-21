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
using Hexalith.Domain.Events;
using Hexalith.Domain.ValueObjets;

using KellermanSoftware.CompareNetObjects;

/// <summary>
/// Class CustomerCommandsConverter.
/// </summary>
public static class CustomerCommandsConverter
{
    /// <summary>
    /// Determines whether the specified registered has changes.
    /// </summary>
    /// <param name="changed">The changed.</param>
    /// <param name="registered">The registered.</param>
    /// <returns>System.Nullable&lt;System.String&gt;.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static string? HasChanges(this ChangeCustomerInformation changed, CustomerRegistered registered)
    {
        ArgumentNullException.ThrowIfNull(changed);
        CompareLogic compareLogic = new();
        ChangeCustomerInformation newValue = new Customer(registered).ToChangeCustomerInformation();
        compareLogic.Config.IgnoreProperty<ChangeCustomerInformation>(x => x.Date);

        ComparisonResult result = compareLogic.Compare(changed, registered);

        return !result.AreEqual ? result.DifferencesString : null;
    }

    /// <summary>
    /// Determines whether the specified registered has changes.
    /// </summary>
    /// <param name="changed">The changed.</param>
    /// <param name="registered">The registered.</param>
    /// <returns>System.Nullable&lt;System.String&gt;.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static string? HasChanges(this RegisterOrChangeCustomer changed, CustomerRegistered registered)
    {
        ArgumentNullException.ThrowIfNull(changed);
        CompareLogic compareLogic = new();
        RegisterOrChangeCustomer newValue = new Customer(registered).ToRegisterOrChangeCustomer();
        compareLogic.Config.IgnoreProperty<ChangeCustomerInformation>(x => x.Date);

        ComparisonResult result = compareLogic.Compare(changed, registered);

        return !result.AreEqual ? result.DifferencesString : null;
    }

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
            new Contact(customer.Contact),
            customer.WarehouseId,
            customer.CommissionSalesGroupId,
            customer.GroupId,
            customer.SalesCurrencyId,
            customer.Date);
    }

    /// <summary>
    /// Converts to command.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <returns>ChangeCustomerInformation.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static ChangeCustomerInformation ToCommand([NotNull] this CustomerInformationChanged customer)
    {
        ArgumentNullException.ThrowIfNull(customer);
        return new ChangeCustomerInformation(
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
            new Contact(register.Contact),
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
            new Contact(change.Contact),
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
            new Contact(customer.Contact),
            customer.WarehouseId,
            customer.CompanyId,
            customer.GroupId,
            customer.SalesCurrencyId,
            customer.Date);
    }

    /// <summary>
    /// Converts to registerorchangecustomer.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <returns>RegisterOrChangeCustomer.</returns>
    /// <exception cref="System.ArgumentNullException">nill.</exception>
    public static RegisterOrChangeCustomer ToRegisterOrChangeCustomer([NotNull] this Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);
        return new RegisterOrChangeCustomer(
            customer.PartitionId,
            customer.CompanyId,
            customer.OriginId,
            customer.Id,
            customer.Name,
            customer.PartyType,
            new Contact(customer.Contact),
            customer.WarehouseId,
            customer.CommissionSalesGroupId,
            customer.GroupId,
            customer.SalesCurrencyId,
            customer.Date);
    }
}