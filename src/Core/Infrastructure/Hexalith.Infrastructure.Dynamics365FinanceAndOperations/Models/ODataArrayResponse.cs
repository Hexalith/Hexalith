// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Models;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Dynamics 365 Finance and Operations entity list response content.
/// </summary>
/// <typeparam name="T">Type of the entituy object</typeparam>
[DataContract]
public class ODataArrayResponse<T>
{
	[DataMember(Name = "@odata.context")]
	[JsonPropertyName("@odata.context")]
	public string? Context { get; set; }

	[DataMember(Name = "message")]
	[JsonPropertyName("message")]
	public string? Message { get; set; }

	[DataMember(Name = "value")]
	[JsonPropertyName("value")]
	public ICollection<T> Values { get; set; } = Array.Empty<T>();
}