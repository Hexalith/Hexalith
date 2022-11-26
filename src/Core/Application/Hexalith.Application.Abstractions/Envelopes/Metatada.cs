// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Application.Abstractions.Envelopes;

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

[DataContract]
public class Metatada : IMetadata
{
	[Obsolete("This constructor is only for serialization purposes.", true)]
	public Metatada()
	{
		CorrelationId = MessageId = UserId = MessageName = MessageTypeName = AggregateId = AggregateName = SessionId = string.Empty;
		Date = DateTimeOffset.MinValue;
	}

	[JsonConstructor]
	public Metatada(
		int majorVersion,
		int minorVersion,
		string userId,
		long sequenceNumber,
		DateTimeOffset date,
		string aggregateName,
		string aggregateId,
		string messageName,
		string messageTypeName,
		string messageId,
		string correlationId,
		int messageMajorVersion,
		int messageMinorVersion,
		string? sessionId,
		IEnumerable<string>? scopes
		)
	{
		MajorVersion = majorVersion;
		MinorVersion = minorVersion;
		SessionId = sessionId;
		UserId = userId;
		SequenceNumber = sequenceNumber;
		Date = date;
		AggregateName = aggregateName;
		AggregateId = aggregateId;
		MessageName = messageName;
		MessageTypeName = messageTypeName;
		MessageId = messageId;
		CorrelationId = correlationId;
		MessageMajorVersion = messageMajorVersion;
		MessageMinorVersion = messageMinorVersion;
		Scopes = scopes;
	}

	public string AggregateId { get; private set; }
	public string AggregateName { get; private set; }
	public string CorrelationId { get; private set; }
	public DateTimeOffset Date { get; private set; }
	public int MajorVersion { get; private set; }
	public string MessageId { get; private set; }
	public int MessageMajorVersion { get; private set; }
	public int MessageMinorVersion { get; private set; }
	public string MessageName { get; private set; }
	public string MessageTypeName { get; private set; }
	public int MinorVersion { get; private set; }
	public IEnumerable<string>? Scopes { get; private set; }
	public long SequenceNumber { get; private set; }
	public string? SessionId { get; private set; }
	public string UserId { get; private set; }
}