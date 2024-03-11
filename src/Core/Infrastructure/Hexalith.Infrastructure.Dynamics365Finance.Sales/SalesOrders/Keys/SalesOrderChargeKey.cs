namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesOrders.Keys;

using System.Runtime.Serialization;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Class SalesOrderLineKey.
/// Implements the <see cref="PerCompanyPrimaryKey" />
/// Implements the <see cref="IPerCompanyPrimaryKey" />
/// Implements the <see cref="IPrimaryKey" />
/// Implements the <see cref="IEquatable{PerCompanyPrimaryKey}" />
/// Implements the <see cref="IEquatable{SalesOrderLineKey}" />.</summary>
[DataContract]
public record SalesOrderChargeKey(
    string? DataAreaId,
    string SalesOrderNumber,
    decimal ChargeLineNumber)
    : PerCompanyPrimaryKey(DataAreaId)
{
}