namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesOrders.Models;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Class SalesOrderHeaderAdditionalUpdate.
/// </summary>
[DataContract]
public class SalesOrderHeaderAdditionalUpdate
{
    /// <summary>Initializes a new instance of the <see cref="SalesOrderHeaderAdditionalUpdate"/> class.</summary>
    /// <param name="phone">The phone.</param>
    /// <param name="deadline">The deadline.</param>
    [JsonConstructor]
    public SalesOrderHeaderAdditionalUpdate(
        string? phone,
        DateTimeOffset? deadline)
    {
        Phone = phone;
        Deadline = deadline;
    }

    /// <summary>Gets the deadline.</summary>
    /// <value>The deadline.</value>
    public DateTimeOffset? Deadline { get; }

    /// <summary>Gets the phone.</summary>
    /// <value>The phone.</value>
    public string? Phone { get; }
}