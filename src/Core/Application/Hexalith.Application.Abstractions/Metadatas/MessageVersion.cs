// Fiveforty S.A. Paris France (2022)

namespace Hexalith.Application.Abstractions.Metadatas;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// The message version
/// </summary>
public class MessageVersion : IMessageVersion
{
	/// <summary>
	/// The major version.
	/// </summary>
	[DataMember(Order = 1)]
	[JsonPropertyOrder(1)]
	public int Major { get; }

	/// <summary>
	/// The minor version
	/// </summary>
	[DataMember(Order = 2)]
	[JsonPropertyOrder(2)]
	public int Minor { get; }
}