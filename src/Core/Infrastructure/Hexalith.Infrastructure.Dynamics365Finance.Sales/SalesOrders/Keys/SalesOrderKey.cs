namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesOrders.Keys;

using System.Runtime.Serialization;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Class SalesOrderKey.
/// Implements the <see cref="PerCompanyPrimaryKey" />
/// Implements the <see cref="IPerCompanyPrimaryKey" />
/// Implements the <see cref="IPrimaryKey" />
/// Implements the <see cref="IEquatable{PerCompanyPrimaryKey}" />
/// Implements the <see cref="IEquatable{SalesOrderKey}" />.
/// </summary>
/// <seealso cref="PerCompanyPrimaryKey" />
/// <seealso cref="IPerCompanyPrimaryKey" />
/// <seealso cref="IPrimaryKey" />
/// <seealso cref="IEquatable{PerCompanyPrimaryKey}" />
/// <seealso cref="IEquatable{SalesOrderKey}" />
[DataContract]
public record SalesOrderKey(
    string? DataAreaId,
    string? SalesOrderNumber)
    : PerCompanyPrimaryKey(DataAreaId)
{
}