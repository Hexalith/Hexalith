// ***********************************************************************
// Assembly         : Christofle.Infrastructure.Dynamics365Finance.RestClient
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-08-2023
// ***********************************************************************
// <copyright file="SalesInvoiceExternalSystemCode.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesInvoices.Entities;

using System.Runtime.Serialization;
using System.Runtime.Serialization.DataContracts;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Class SalesInvoiceExternalCode.
/// Implements the <see cref="ODataElement" />
/// Implements the <see cref="IEquatable{ODataElement}" />
/// Implements the <see cref="IODataElement" />
/// Implements the <see cref="IEquatable{SalesInvoiceExternalCode}" />.
/// </summary>
/// <seealso cref="ODataElement" />
/// <seealso cref="IEquatable{ODataElement}" />
/// <seealso cref="IODataElement" />
/// <seealso cref="IEquatable{SalesInvoiceExternalCode}" />
[DataContract]
public record SalesInvoiceExternalSystemCode
(
    string? Etag,
    string DataAreaId,
    [property: DataMember(Order = 3)] string? System,
    [property: DataMember(Order = 4)] string? SalesInvoiceAccountNumber,
    [property: DataMember(Order = 5)] string? ExternalCode)

: ODataElement(Etag, DataAreaId), IODataElement
{
    /// <summary>
    /// Entities the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string EntityName() => "FFYSalesInvoiceExternalSystemCodes";
}