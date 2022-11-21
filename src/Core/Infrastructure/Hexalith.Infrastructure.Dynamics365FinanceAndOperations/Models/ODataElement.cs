// Fiveforty S.A. Paris France (2022)

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Models;

using System.Text.Json.Serialization;

public abstract record ODataElement
{
	[JsonConstructor]
	protected ODataElement(string etag, string dataAreaId)
	{
		Etag = etag;
		DataAreaId = dataAreaId;
	}
	[JsonPropertyName("@odata.etag")]
	public string Etag { get; }

	[JsonPropertyName("dataAreaId")]
	public string DataAreaId { get; }
}