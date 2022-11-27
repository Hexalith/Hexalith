// Fiveforty S.A. Paris France (2022)

namespace Hexalith.Application.Abstractions.Metadatas;

/// <summary>
/// The aggregate metadata.
/// </summary>
internal class AggregateMetadata : IAggregateMetaData
{
	/// <summary>
	/// Initializes a new instance of the <see cref="AggregateMetadata" /> class.
	/// </summary>
	[Obsolete("This constructor is only for serialization purposes.", true)]
	public AggregateMetadata() => Id = Name = string.Empty;

	/// <summary>
	/// Initializes a new instance of the <see cref="AggregateMetadata" /> class.
	/// </summary>
	/// <param name="id">The aggregate identifier</param>
	/// <param name="name">The aggregate name</param>
	public AggregateMetadata(string id, string name)
	{
		Id = id;
		Name = name;
	}

	/// <summary>
	/// The aggregate identifier.
	/// </summary>
	public string Id { get; }

	/// <summary>
	/// The aggregate name.
	/// </summary>
	public string Name { get; }
}