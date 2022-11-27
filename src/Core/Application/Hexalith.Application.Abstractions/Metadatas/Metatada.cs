// Fiveforty S.A. Paris France (2022)

namespace Hexalith.Application.Abstractions.Metadatas;

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
		Message = new MessageMetadata();
		Version = new MetadataVersion();
		Context = new ContextMetadata();
		Date = DateTimeOffset.MinValue;
	}

	[JsonConstructor]
	public Metatada(
		IMetadataVersion version,
		IMessageMetadata message,
		IContextMetadata context,
		DateTimeOffset date,
		IEnumerable<string>? scopes
		)
	{
		Version = version;
		Message = message;
		Context = context;
		Date = date;
		Scopes = scopes;
	}

	public IContextMetadata Context { get; private set; }
	public DateTimeOffset Date { get; private set; }
	public IMessageMetadata Message { get; private set; }
	public IEnumerable<string>? Scopes { get; private set; }
	public IMetadataVersion Version { get; private set; }
}