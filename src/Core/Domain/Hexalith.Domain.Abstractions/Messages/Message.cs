// <copyright file="Message.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Abstractions.Messages;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Base class for messages.
/// </summary>
[DataContract]
public abstract class Message : IMessage
{
	/// <inheritdoc/>
	[IgnoreDataMember]
	[JsonIgnore]
	public string AggregateId => GetAggregateId();

	/// <inheritdoc/>
	[IgnoreDataMember]
	[JsonIgnore]
	public string AggregateName => GetAggregateName();

	/// <inheritdoc/>
	[IgnoreDataMember]
	[JsonIgnore]
	public int MajorVersion => GetMajorVersion();

	/// <inheritdoc/>
	[IgnoreDataMember]
	[JsonIgnore]
	public string MessageName => GetMessageName();

	/// <inheritdoc/>
	[IgnoreDataMember]
	[JsonIgnore]
	public int MinorVersion => GetMinorVersion();

	/// <summary>
	/// Get the aggregate identifier.
	/// </summary>
	/// <returns>The identifier.</returns>
	protected abstract string GetAggregateId();

	/// <summary>
	/// Get the aggregate name.
	/// </summary>
	/// <returns>The name.</returns>
	protected abstract string GetAggregateName();

	/// <summary>
	/// Get the message major version.
	/// </summary>
	/// <returns>The major version.</returns>
	protected virtual int GetMajorVersion()
	{
		return 0;
	}

	/// <summary>
	/// Get the message name.
	/// </summary>
	/// <returns>The name.</returns>
	protected abstract string GetMessageName();

	/// <summary>
	/// Gets the message minor version.
	/// </summary>
	/// <returns>The minor version.</returns>
	protected virtual int GetMinorVersion()
	{
		return 0;
	}
}