// Fiveforty S.A. Paris France (2022)

namespace Hexalith.Domain.Abstractions.Messages;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Base class for messages.
/// </summary>
[DataContract]
public abstract class Message : IMessage
{
	[IgnoreDataMember]
	[JsonIgnore]
	public string AggregateId => GetAggregateId();

	[IgnoreDataMember]
	[JsonIgnore]
	public string AggregateName => GetAggregateName();

	[IgnoreDataMember]
	[JsonIgnore]
	public int MajorVersion => GetMajorVersion();

	[IgnoreDataMember]
	[JsonIgnore]
	public string MessageName => GetMessageName();

	[IgnoreDataMember]
	[JsonIgnore]
	public int MinorVersion => GetMinorVersion();

	protected abstract string GetAggregateId();

	protected abstract string GetAggregateName();

	protected virtual int GetMajorVersion() => 0;

	protected abstract string GetMessageName();

	protected virtual int GetMinorVersion() => 0;
}