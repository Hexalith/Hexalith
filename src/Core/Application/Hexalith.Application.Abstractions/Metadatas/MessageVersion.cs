// <copyright file="MessageVersion.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Metadatas;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// The message version.
/// </summary>
public class MessageVersion : IMessageVersion
{
	/// <summary>
	/// Gets the major version.
	/// </summary>
	[DataMember(Order = 1)]
	[JsonPropertyOrder(1)]
	public int Major { get; }

	/// <summary>
	/// Gets the minor version.
	/// </summary>
	[DataMember(Order = 2)]
	[JsonPropertyOrder(2)]
	public int Minor { get; }
}