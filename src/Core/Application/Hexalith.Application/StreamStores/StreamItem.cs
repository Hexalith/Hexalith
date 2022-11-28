// <copyright file="StreamItem.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.StreamStores;

using Hexalith.Application.Abstractions.StreamStores;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Persited stream item.
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
	/// Initializes a new instance of the <see cref="StreamItem"/> class.
	/// Initializer for serializers that require a parameterless constructor.
	/// </summary>
	[Obsolete("This constructor is only for serialization purposes.", true)]
	public StreamItem()
	{
		Message = new DataFragment();
	}

	/// <summary>
	/// Gets message data.
	/// </summary>
	public IDataFragment Message { get; }

	/// <summary>
	/// Gets or sets the stream sequence number.
	/// </summary>
	[DataMember(Order = 1)]
	[JsonPropertyOrder(1)]
	public long Sequence { get; set; }
}