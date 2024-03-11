namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesOrders.Models;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Class SalesOrderHeaderAdditionalUpdate.
/// </summary>
[DataContract]
public class SalesOrderHeaderAdditionalUpdateOrigin
{
    /// <summary>Initializes a new instance of the <see cref="SalesOrderHeaderAdditionalUpdateOrigin"/> class.</summary>
    /// <param name="salesOriginId">The sales origin.</param>
    [JsonConstructor]
    public SalesOrderHeaderAdditionalUpdateOrigin(string? salesOriginId)
    {
        SalesOriginId = salesOriginId;
    }

    /// <summary>Gets the sales order origin code.</summary>
    /// <value>The sales order origin code.</value>
    public string? SalesOriginId { get; }
}