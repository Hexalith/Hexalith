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
    string PackingSlipId,
    string SalesId,
    DateTimeOffset DeliveryDate,
    decimal LineNumber,
    int CustomersLineNumber,
    long InvoiceJourRecId,
    long InvoiceTransRecId,
    decimal Quantity)
    : ODataElement(Etag, DataAreaId), IODataElement
{
    /// <summary>
    /// Entities the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string EntityName()
    {
        return "BusinessDocumentStockedPackingSlipLines";
    }
}