// <copyright file="AggregateMetadata.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Metadatas;

/// <summary>
/// The aggregate metadata.
/// </summary>
public class AggregateMetadata : IAggregateMetaData
{
	/// <summary>
	/// Initializes a new instance of the <see cref="AggregateMetadata" /> class.
	/// </summary>
	[Obsolete("This constructor is only for serialization purposes.", true)]
	public AggregateMetadata()
	{
		Id = Name = string.Empty;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="AggregateMetadata" /> class.
	/// </summary>
	/// <param name="id">The aggregate identifier.</param>
	/// <param name="name">The aggregate name.</param>
	public AggregateMetadata(string id, string name)
	{
		Id = id;
		Name = name;
	}

	/// <summary>
	/// Gets the aggregate identifier.
	/// </summary>
	public string Id { get; }

	/// <summary>
	/// Gets the aggregate name.
	/// </summary>
	public string Name { get; }
}