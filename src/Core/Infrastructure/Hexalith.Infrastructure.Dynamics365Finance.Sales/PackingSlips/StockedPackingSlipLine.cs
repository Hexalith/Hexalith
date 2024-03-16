// <copyright file="StockedPackingSlipLine.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.PackingSlips;

using System.Runtime.Serialization;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Class PackingSlipLine.
/// Implements the <see cref="ODataElement" />
/// Implements the <see cref="IEquatable{ODataElement}" />
/// Implements the <see cref="IODataElement" />
/// Implements the <see cref="IEquatable{Christofle.Infrastructure.Dynamics365Finance.RestClient.PackingSlips.PackingSlipLine}" />.
/// </summary>
/// <seealso cref="ODataElement" />
/// <seealso cref="IEquatable{ODataElement}" />
/// <seealso cref="IODataElement" />
/// <seealso cref="IEquatable{Christofle.Infrastructure.Dynamics365Finance.RestClient.PackingSlips.PackingSlipLine}" />
[DataContract]
public record StockedPackingSlipLine(
    string Etag,
    string DataAreaId,
    [property: DataMember(Order = 3)] string PackingSlipId,
    [property: DataMember(Order = 4)] string SalesId,
    [property: DataMember(Order = 5)] DateTimeOffset DeliveryDate,
    [property: DataMember(Order = 6)] decimal LineNumber,
    [property: DataMember(Order = 7)] int CustomersLineNumber,
    [property: DataMember(Order = 8)] long InvoiceJourRecId,
    [property: DataMember(Order = 9)] long InvoiceTransRecId,
    [property: DataMember(Order = 10)] decimal Quantity)
    : ODataElement(Etag, DataAreaId), IODataElement
{
    /// <summary>
    /// Entities the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string EntityName() => "BusinessDocumentStockedPackingSlipLines";
}