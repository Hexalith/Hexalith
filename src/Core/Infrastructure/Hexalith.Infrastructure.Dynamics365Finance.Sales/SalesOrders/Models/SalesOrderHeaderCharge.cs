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
    string SalesOrderNumber,
    decimal ChargeLineNumber,
    string ChargeDescription,
    string WillInvoiceProcessingKeepCharge,
    string SalesTaxGroupCode,
    string ChargeAccountingCurrencyCode,
    string SalesTaxItemGroupCode,
    string SalesChargeCode,
    string ChargeCategory,
    string IsIntercompanyCharge,
    string OverrideSalesTax,
    decimal ChargePercentage,
    decimal FixedChargeAmount,
    decimal ExternalChargeAmount)
    : ODataElement(Etag, DataAreaId), IODataElement
{
    /// <summary>
    /// Entities the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string EntityName()
    {
        return "FFYSalesOrderHeaderChargesV2";
    }
}