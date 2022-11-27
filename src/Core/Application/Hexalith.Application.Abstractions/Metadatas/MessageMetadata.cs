// Fiveforty S.A. Paris France (2022)

namespace Hexalith.Application.Abstractions.Metadatas;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

[DataContract]
public class MessageMetadata : IMessageMetadata
{
	[Obsolete("This constructor is only for serialization purposes.", true)]
	public MessageMetadata()
	{
		Id = Name = string.Empty;
		Version = new MessageVersion();
		Aggregate = new AggregateMetadata();
	}

	public MessageMetadata(string id, string name, IMessageVersion version, IAggregateMetaData aggregate)
	{
		Id = id;
		Name = name;
		Version = version;
		Aggregate = aggregate;
	}

	[DataMember(Order = 4)]
	[JsonPropertyOrder(4)]
	public IAggregateMetaData Aggregate { get; }

	[DataMember(Order = 1)]
	[JsonPropertyOrder(1)]
	public string Id { get; }

	[DataMember(Order = 2)]
	[JsonPropertyOrder(2)]
	public string Name { get; }

	[DataMember(Order = 3)]
	[JsonPropertyOrder(3)]
	public IMessageVersion Version { get; }
}