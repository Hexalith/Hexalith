// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Application.StreamStores;

using Hexalith.Application.Abstractions.StreamStores;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Persited stream item
/// </summary>
[DataContract]
public class StreamItem : IStreamItem
{
	/// <summary>
	/// Initializes a new instance of the <see cref="StreamItem" /> class.
	/// </summary>
	[JsonConstructor]
	public StreamItem(long sequence, IDataFragment message)
	{
		Sequence = sequence;
		Message = message;
	}

	/// <summary>
	/// Initializer for serializers that require a parameterless constructor
	/// </summary>
	[Obsolete("This constructor is only for serialization purposes.", true)]
	public StreamItem() => Message = new DataFragment();

	/// <summary>
	/// Message data
	/// </summary>
	public IDataFragment Message { get; }

	/// <summary>
	/// Gets the stream sequence number
	/// </summary>
	[DataMember(Order = 1)]
	[JsonPropertyOrder(1)]
	public long Sequence { get; set; }
}