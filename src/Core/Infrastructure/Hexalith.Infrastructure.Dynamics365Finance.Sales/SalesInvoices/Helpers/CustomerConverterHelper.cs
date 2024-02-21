//// ***********************************************************************
//// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Sales
//// Author           : Jérôme Piquot
//// Created          : 11-21-2023
////
//// Last Modified By : Jérôme Piquot
//// Last Modified On : 12-20-2023
//// ***********************************************************************
// <copyright file="CustomerConverterHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
//// <summary></summary>
//// ***********************************************************************

// namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesInvoices.Helpers;

// using System;
// using System.Diagnostics.CodeAnalysis;
// using System.Runtime.CompilerServices;

// using Hexalith.Application.Sales.Commands;
// using Hexalith.Domain.Events;
// using Hexalith.Domain.ValueObjets;
// using Hexalith.Extensions.Common;
// using Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesInvoices.Entities;
// using Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesInvoices.IntegrationEvents;

///// <summary>
///// Class SalesInvoiceConverter.
///// </summary>
// public static class SalesInvoiceConverterHelper
// {
// /// <summary>
//    /// Converts to customerregisteredevent.
//    /// </summary>
//    /// <param name="customer">The customer.</param>
//    /// <param name="partitionId">The partition identifier.</param>
//    /// <param name="companyId">The company identifier.</param>
//    /// <param name="originId">The origin identifier.</param>
//    /// <param name="customerId">The customer identifier.</param>
//    /// <param name="date">The date.</param>
//    /// <param name="postBox">The post box.</param>
//    /// <param name="stateName">Name of the state.</param>
//    /// <param name="countryName">Name of the country.</param>
//    /// <param name="phone">The phone.</param>
//    /// <param name="mobile">The mobile.</param>
//    /// <param name="title">The title.</param>
//    /// <param name="birthDate">The birth date.</param>
//    /// <returns>Hexalith.Domain.Events.SalesInvoicePosted.</returns>
//    public static SalesInvoicePostedBusinessEvent ToSalesInvoicePostedEvent(
//        [NotNull] this SalesInvoiceV3 customer,
//        string partitionId,
//        string companyId,
//        string originId,
//        string customerId,
//        DateTimeOffset date,
//        string? postBox = null,
//        string? stateName = null,
//        string? countryName = null,
//        string? phone = null,
//        string? mobile = null,
//        string? title = null,
//        DateTimeOffset? birthDate = null)
//    {
//        ArgumentNullException.ThrowIfNull(customer);
//        ArgumentException.ThrowIfNullOrWhiteSpace(customer.SalesInvoiceAccount);
//        ArgumentException.ThrowIfNullOrWhiteSpace(customer.OrganizationName);
//        SalesInvoicePostedBusinessEvent registered = new(
//            partitionId,
//            companyId,
//            originId,
//            customerId,
//            customer.OrganizationName,
//            ToPartyType(customer.PartyType ?? string.Empty),
//            new Contact(
//                new Person(
//                    customer.OrganizationName,
//                    customer.PersonFirstName,
//                    customer.PersonLastName,
//                    title,
//                    birthDate,
//                    ToGender(customer.PersonGender)),
//                new PostalAddress(
//                    customer.AddressDescription,
//                    customer.AddressDescription,
//                    customer.AddressStreetNumber,
//                    customer.AddressStreet,
//                    postBox,
//                    customer.AddressZipCode,
//                    customer.AddressCity,
//                    customer.AddressCounty,
//                    customer.AddressState,
//                    stateName,
//                    customer.AddressCountryRegionId,
//                    countryName,
//                    customer.AddressCountryRegionISOCode,
//                    null,
//                    null,
//                    null,
//                    null),
//                customer.PrimaryContactEmail,
//                customer.PrimaryContactPhoneIsMobile != "Yes" ? customer.PrimaryContactPhone : phone,
//                customer.PrimaryContactPhoneIsMobile == "Yes" ? customer.PrimaryContactPhone : mobile),
//            customer.WarehouseId,
//            customer.CommissionSalesGroupId,
//            customer.SalesInvoiceGroupId,
//            customer.SalesCurrencyCode,
//            date);
//        return registered;
//    }

// }