// <copyright file="SalesOrderHeaderCharge.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesOrders.Models;

using System.Runtime.Serialization;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Class SalesOrderHeaderAdditional.
/// Implements the <see cref="ODataElement" />
/// Implements the <see cref="IEquatable{ODataElement}" />
/// Implements the <see cref="IODataElement" />
/// Implements the <see cref="IEquatable{SalesOrderHeaderAdditional}" />.
/// </summary>
/// <seealso cref="ODataElement" />
/// <seealso cref="IEquatable{ODataElement}" />
/// <seealso cref="IODataElement" />
/// <seealso cref="IEquatable{SalesOrderHeaderAdditional}" />
[DataContract]
public record SalesOrderHeaderCharge(
    string Etag,
    string DataAreaId,
    [property: DataMember(Order = 3)] string SalesOrderNumber,
    [property: DataMember(Order = 4)] decimal ChargeLineNumber,
    [property: DataMember(Order = 5)] string ChargeDescription,
    [property: DataMember(Order = 6)] string WillInvoiceProcessingKeepCharge,
    [property: DataMember(Order = 7)] string SalesTaxGroupCode,
    [property: DataMember(Order = 8)] string ChargeAccountingCurrencyCode,
    [property: DataMember(Order = 9)] string SalesTaxItemGroupCode,
    [property: DataMember(Order = 10)] string SalesChargeCode,
    [property: DataMember(Order = 11)] string ChargeCategory,
    [property: DataMember(Order = 12)] string IsIntercompanyCharge,
    [property: DataMember(Order = 13)] string OverrideSalesTax,
    [property: DataMember(Order = 14)] decimal ChargePercentage,
    [property: DataMember(Order = 15)] decimal FixedChargeAmount,
    [property: DataMember(Order = 16)] decimal ExternalChargeAmount)
    : ODataElement(Etag, DataAreaId), IODataElement
{
    /// <summary>
    /// Entities the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string EntityName() => "FFYSalesOrderHeaderChargesV2";
}