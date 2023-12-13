// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Parties
// Author           : Jérôme Piquot
// Created          : 11-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-13-2023
// ***********************************************************************
// <copyright file="CustomerConverter.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Helpers;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using Hexalith.Domain.Events;
using Hexalith.Domain.ValueObjets;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Entities;

/// <summary>
/// Class CustomerConverter.
/// </summary>
public static class CustomerConverter
{
    /// <summary>
    /// Gets the changes.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <param name="e">The e.</param>
    /// <returns>System.Collections.Generic.Dictionary&lt;string, object?&gt;.</returns>
    public static Dictionary<string, object?> GetChanges([NotNull] this CustomerBase customer, [NotNull] CustomerInformationChanged e)
    {
        ArgumentNullException.ThrowIfNull(customer);
        ArgumentNullException.ThrowIfNull(e);
        Dictionary<string, object?> changes = [];

        changes.AddChanges(customer.PersonBirthDay, e.Contact?.Person?.BirthDate?.Day);
        changes.AddChanges(customer.PersonBirthMonth, e.Contact?.Person?.BirthDate?.Month);
        changes.AddChanges(customer.PersonBirthYear, e.Contact?.Person?.BirthDate?.Year);
        changes.AddChanges(customer.PersonPersonalTitle, e.Contact?.Person?.Title);

        return changes;
    }

    /// <summary>
    /// Gets the changes.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <param name="e">The e.</param>
    /// <returns>System.Collections.Generic.Dictionary&lt;string, object?&gt;.</returns>
    public static Dictionary<string, object?> GetChanges([NotNull] this CustomerV3 customer, [NotNull] CustomerInformationChanged e)
    {
        ArgumentNullException.ThrowIfNull(customer);
        ArgumentNullException.ThrowIfNull(e);
        Dictionary<string, object?> changes = [];

        changes.AddChanges(customer.OrganizationName, e.Name);
        changes.AddChanges(customer.AddressCity, e.Contact?.PostalAddress?.City);
        changes.AddChanges(customer.AddressCountryRegionId, e.Contact?.PostalAddress?.CountryId);
        changes.AddChanges(customer.AddressCountryRegionISOCode, e.Contact?.PostalAddress?.CountryIso2);
        changes.AddChanges(customer.AddressCounty, e.Contact?.PostalAddress?.CountyId);
        changes.AddChanges(customer.AddressState, e.Contact?.PostalAddress?.StateId);
        changes.AddChanges(customer.AddressStreet, e.Contact?.PostalAddress?.Street);
        changes.AddChanges(customer.AddressStreetNumber, e.Contact?.PostalAddress?.StreetNumber);
        changes.AddChanges(customer.AddressZipCode, e.Contact?.PostalAddress?.ZipCode);
        changes.AddChanges(customer.AddressDescription, e.Contact?.PostalAddress?.Name);
        changes.AddChanges(customer.PersonFirstName, e.Contact?.Person?.FirstName);
        changes.AddChanges(customer.PersonLastName, e.Contact?.Person?.LastName);
        changes.AddChanges(customer.PersonGender, ToDynamicsGender(e.Contact?.Person?.Gender));
        changes.AddChanges(customer.PrimaryContactPhoneExtension, e.Contact?.Mobile ?? e.Contact?.Phone);
        changes.AddChanges(customer.PrimaryContactPhoneIsMobile, e.Contact?.Mobile == null ? "No" : "Yes");
        changes.AddChanges(customer.PrimaryContactEmail, e.Contact?.Email);
        changes.AddChanges(customer.CustomerGroupId, e.GroupId);
        changes.AddChanges(customer.SalesCurrencyCode, e.SalesCurrencyId);
        changes.AddChanges(customer.PartyType, ToDynamicsPartyType(e.PartyType));
        changes.AddChanges(customer.CommissionSalesGroupId, e.CommissionSalesGroupId);
        changes.AddChanges(customer.WarehouseId, e.WarehouseId);

        return changes;
    }

    /// <summary>
    /// Converts to customerchangedevent.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="date">The date.</param>
    /// <param name="postBox">The post box.</param>
    /// <param name="stateName">Name of the state.</param>
    /// <param name="countryName">Name of the country.</param>
    /// <param name="phone">The phone.</param>
    /// <param name="mobile">The mobile.</param>
    /// <param name="title">The title.</param>
    /// <param name="birthDate">The birth date.</param>
    /// <returns>Hexalith.Domain.Events.CustomerInformationChanged.</returns>
    public static CustomerInformationChanged ToCustomerChangedEvent(
        [NotNull] this CustomerV3 customer,
        string partitionId,
        string originId,
        DateTimeOffset date,
        string? postBox = null,
        string? stateName = null,
        string? countryName = null,
        string? phone = null,
        string? mobile = null,
        string? title = null,
        DateTimeOffset? birthDate = null)
    {
        ArgumentNullException.ThrowIfNull(customer);
        ArgumentException.ThrowIfNullOrEmpty(customer.CustomerAccount);
        ArgumentException.ThrowIfNullOrEmpty(customer.OrganizationName);
        CustomerInformationChanged changed = new(
            partitionId,
            customer.DataAreaId,
            originId,
            customer.CustomerAccount,
            customer.OrganizationName,
            ToPartyType(customer.PartyType ?? string.Empty),
            new Contact(
                new Person(
                    customer.OrganizationName,
                    customer.PersonFirstName,
                    customer.PersonLastName,
                    title,
                    birthDate,
                    ToGender(customer.PersonGender)),
                new PostalAddress(
                    customer.AddressDescription,
                    customer.AddressDescription,
                    customer.AddressStreetNumber,
                    customer.AddressStreet,
                    postBox,
                    customer.AddressZipCode,
                    customer.AddressCity,
                    customer.AddressCounty,
                    customer.AddressState,
                    stateName,
                    customer.AddressCountryRegionId,
                    countryName,
                    customer.AddressCountryRegionISOCode),
                customer.PrimaryContactEmail,
                customer.PrimaryContactPhoneIsMobile != "Yes" ? customer.PrimaryContactPhoneExtension : phone,
                customer.PrimaryContactPhoneIsMobile == "Yes" ? customer.PrimaryContactPhoneExtension : mobile),
            customer.WarehouseId,
            customer.CommissionSalesGroupId,
            customer.CustomerGroupId,
            customer.SalesCurrencyCode,
            date);
        return changed;
    }

    /// <summary>
    /// Converts to customerregisteredevent.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="date">The date.</param>
    /// <param name="postBox">The post box.</param>
    /// <param name="stateName">Name of the state.</param>
    /// <param name="countryName">Name of the country.</param>
    /// <param name="phone">The phone.</param>
    /// <param name="mobile">The mobile.</param>
    /// <param name="title">The title.</param>
    /// <param name="birthDate">The birth date.</param>
    /// <returns>Hexalith.Domain.Events.CustomerRegistered.</returns>
    public static CustomerRegistered ToCustomerRegisteredEvent(
        [NotNull] this CustomerV3 customer,
        string partitionId,
        string originId,
        DateTimeOffset date,
        string? postBox = null,
        string? stateName = null,
        string? countryName = null,
        string? phone = null,
        string? mobile = null,
        string? title = null,
        DateTimeOffset? birthDate = null)
    {
        ArgumentNullException.ThrowIfNull(customer);
        ArgumentException.ThrowIfNullOrEmpty(customer.CustomerAccount);
        ArgumentException.ThrowIfNullOrEmpty(customer.OrganizationName);
        CustomerRegistered registered = new(
            partitionId,
            customer.DataAreaId,
            originId,
            customer.CustomerAccount,
            customer.OrganizationName,
            ToPartyType(customer.PartyType ?? string.Empty),
            new Contact(
                new Person(
                    customer.OrganizationName,
                    customer.PersonFirstName,
                    customer.PersonLastName,
                    title,
                    birthDate,
                    ToGender(customer.PersonGender)),
                new PostalAddress(
                    customer.AddressDescription,
                    customer.AddressDescription,
                    customer.AddressStreetNumber,
                    customer.AddressStreet,
                    postBox,
                    customer.AddressZipCode,
                    customer.AddressCity,
                    customer.AddressCounty,
                    customer.AddressState,
                    stateName,
                    customer.AddressCountryRegionId,
                    countryName,
                    customer.AddressCountryRegionISOCode),
                customer.PrimaryContactEmail,
                customer.PrimaryContactPhoneIsMobile != "Yes" ? customer.PrimaryContactPhoneExtension : phone,
                customer.PrimaryContactPhoneIsMobile == "Yes" ? customer.PrimaryContactPhoneExtension : mobile),
            customer.WarehouseId,
            customer.CommissionSalesGroupId,
            customer.CustomerGroupId,
            customer.SalesCurrencyCode,
            date);
        return registered;
    }

    /// <summary>
    /// Converts to dynamics 365 finance customer.
    /// </summary>
    /// <param name="customerRegistered">The customer registered.</param>
    /// <param name="customerAccount">The customer account.</param>
    /// <returns>CustomerV3.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static CustomerV3 ToDynamics365FinanceCustomer(
        [NotNull] this CustomerRegistered customerRegistered,
        string? customerAccount = null)
    {
        ArgumentNullException.ThrowIfNull(customerRegistered);
        CustomerV3 customerV3 = new(customerRegistered.CompanyId)
        {
            CustomerAccount = customerAccount ?? customerRegistered.Id,
            OrganizationName = customerRegistered.Name,
            AddressCity = customerRegistered.Contact.PostalAddress?.City,
            AddressCountryRegionId = customerRegistered.Contact.PostalAddress?.CountryId,
            AddressCountryRegionISOCode = customerRegistered.Contact.PostalAddress?.CountryIso2,
            AddressState = customerRegistered.Contact.PostalAddress?.StateId,
            AddressCounty = customerRegistered.Contact.PostalAddress?.CountyId,
            AddressStreetNumber = customerRegistered.Contact.PostalAddress?.StreetNumber,
            AddressStreet = customerRegistered.Contact.PostalAddress?.Street,
            AddressZipCode = customerRegistered.Contact.PostalAddress?.ZipCode,
            AddressDescription = customerRegistered.Contact.PostalAddress?.Name,
            PersonFirstName = customerRegistered.Contact.Person?.FirstName,
            PersonLastName = customerRegistered.Contact.Person?.LastName,
            PersonGender = ToDynamicsGender(customerRegistered.Contact.Person?.Gender),
            PrimaryContactPhoneExtension = customerRegistered.Contact?.Mobile ?? customerRegistered.Contact?.Phone,
            PrimaryContactPhoneIsMobile = customerRegistered.Contact?.Mobile == null ? "No" : "Yes",
            PrimaryContactEmail = customerRegistered.Contact?.Email,
            CustomerGroupId = customerRegistered.GroupId,
            SalesCurrencyCode = customerRegistered.SalesCurrencyId,
            PartyType = ToDynamicsPartyType(customerRegistered.PartyType),
            CommissionSalesGroupId = customerRegistered.CommissionSalesGroupId,
            WarehouseId = customerRegistered.WarehouseId,
        };
        return customerV3;
    }

    /// <summary>
    /// Converts to dynamics 365 finance customer.
    /// </summary>
    /// <param name="customerChanged">The customer changed.</param>
    /// <param name="customerAccount">The customer account.</param>
    /// <returns>CustomerV3.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static CustomerV3 ToDynamics365FinanceCustomer(
        [NotNull] this CustomerInformationChanged customerChanged,
        string? customerAccount = null)
    {
        ArgumentNullException.ThrowIfNull(customerChanged);
        CustomerV3 customerV3 = new(customerChanged.CompanyId)
        {
            CustomerAccount = customerAccount ?? customerChanged.Id,
            OrganizationName = customerChanged.Name,
            AddressCity = customerChanged.Contact.PostalAddress?.City,
            AddressCountryRegionId = customerChanged.Contact.PostalAddress?.CountryId,
            AddressCountryRegionISOCode = customerChanged.Contact.PostalAddress?.CountryIso2,
            AddressCounty = customerChanged.Contact.PostalAddress?.CountyId,
            AddressState = customerChanged.Contact.PostalAddress?.StateId,
            AddressStreet = customerChanged.Contact.PostalAddress?.Street,
            AddressStreetNumber = customerChanged.Contact.PostalAddress?.StreetNumber,
            AddressZipCode = customerChanged.Contact.PostalAddress?.ZipCode,
            AddressDescription = customerChanged.Contact.PostalAddress?.Name,
            PersonFirstName = customerChanged.Contact.Person?.FirstName,
            PersonLastName = customerChanged.Contact.Person?.LastName,
            PersonGender = ToDynamicsGender(customerChanged.Contact.Person?.Gender),
            PrimaryContactPhoneExtension = customerChanged.Contact?.Mobile ?? customerChanged.Contact?.Phone,
            PrimaryContactPhoneIsMobile = customerChanged.Contact?.Mobile == null ? "No" : "Yes",
            PrimaryContactEmail = customerChanged.Contact?.Email,
            CustomerGroupId = customerChanged.GroupId,
            SalesCurrencyCode = customerChanged.SalesCurrencyId,
            PartyType = ToDynamicsPartyType(customerChanged.PartyType),
            CommissionSalesGroupId = customerChanged.CommissionSalesGroupId,
            WarehouseId = customerChanged.WarehouseId,
        };
        return customerV3;
    }

    /// <summary>
    /// Converts to dynamics365financecustomercreate.
    /// </summary>
    /// <param name="customerRegistered">The customer registered.</param>
    /// <returns>Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Entities.CustomerV3Create.</returns>
    public static CustomerV3Create ToDynamics365FinanceCustomerCreate(
                [NotNull] this CustomerRegistered customerRegistered, string? organizationName = null)
    {
        ArgumentNullException.ThrowIfNull(customerRegistered);
        CustomerV3Create customerV3 = new()
        {
            DataAreaId = customerRegistered.CompanyId,
            OrganizationName = organizationName ?? customerRegistered.Name,
            AddressCity = customerRegistered.Contact.PostalAddress?.City,
            AddressCountryRegionId = customerRegistered.Contact.PostalAddress?.CountryId,
            AddressStreet = customerRegistered.Contact.PostalAddress?.Street,
            AddressZipCode = customerRegistered.Contact.PostalAddress?.ZipCode,
            AddressDescription = customerRegistered.Contact.PostalAddress?.Name,
            PersonFirstName = customerRegistered.Contact.Person?.FirstName,
            PersonLastName = customerRegistered.Contact.Person?.LastName,
            PersonGender = ToDynamicsGender(customerRegistered.Contact.Person?.Gender),
            PrimaryContactPhoneExtension = customerRegistered.Contact?.Mobile ?? customerRegistered.Contact?.Phone,
            PrimaryContactPhoneIsMobile = customerRegistered.Contact?.Mobile == null ? "No" : "Yes",
            PrimaryContactEmail = customerRegistered.Contact?.Email,
            CustomerGroupId = customerRegistered.GroupId,
            SalesCurrencyCode = customerRegistered.SalesCurrencyId,
            PartyType = ToDynamicsPartyType(customerRegistered.PartyType),
            CommissionSalesGroupId = customerRegistered.CommissionSalesGroupId,
            WarehouseId = customerRegistered.WarehouseId,
        };
        return customerV3;
    }

    /// <summary>
    /// Converts to dynamics gender.
    /// </summary>
    /// <param name="gender">The gender.</param>
    /// <returns>string?.</returns>
    public static string? ToDynamicsGender(Gender? gender)
    {
        return gender switch
        {
            Gender.Male => nameof(Gender.Male),
            Gender.Female => nameof(Gender.Female),
            null => null,
            _ => nameof(Gender.Other),
        };
    }

    /// <summary>
    /// Converts to dynamics party type.
    /// </summary>
    /// <param name="partyType">Type of the party.</param>
    /// <returns>System.String.</returns>
    public static string ToDynamicsPartyType(PartyType partyType)
    {
        return partyType switch
        {
            PartyType.Person => nameof(PartyType.Person),
            PartyType.Organisation => nameof(PartyType.Organisation),
            _ => nameof(PartyType.Other),
        };
    }

    /// <summary>
    /// Converts to gender.
    /// </summary>
    /// <param name="gender">The gender.</param>
    /// <returns>Hexalith.Domain.ValueObjets.Gender?.</returns>
    public static Gender? ToGender(string? gender)
    {
        return gender switch
        {
            nameof(Gender.Male) => Gender.Male,
            nameof(Gender.Female) => Gender.Female,
            null => null,
            _ => Gender.Other,
        };
    }

    /// <summary>
    /// Converts to party type.
    /// </summary>
    /// <param name="partyType">Type of the party.</param>
    /// <returns>PartyType.</returns>
    public static PartyType ToPartyType(string partyType)
    {
        return partyType switch
        {
            nameof(PartyType.Person) => PartyType.Person,
            nameof(PartyType.Organisation) => PartyType.Organisation,
            _ => PartyType.Other,
        };
    }

    /// <summary>
    /// Adds the changes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="changes">The changes.</param>
    /// <param name="originalValue">The original value.</param>
    /// <param name="newValue">The new value.</param>
    /// <param name="fieldName">Name of the field.</param>
    private static void AddChanges<T>(this Dictionary<string, object?> changes, T? originalValue, T? newValue, [CallerArgumentExpression("originalValue")] string? fieldName = null)
    {
        if (originalValue == null && newValue == null)
        {
            return;
        }

        if (originalValue != null && originalValue.Equals(newValue))
        {
            return;
        }

        string? name = (string.IsNullOrWhiteSpace(fieldName)
            ? null
            : fieldName.Split(".").LastOrDefault()) ?? throw new InvalidOperationException("Invalid field name");
        if (newValue == null)
        {
            changes.Add(name, default(T));
        }
        else
        {
            changes.Add(name, newValue);
        }
    }
}