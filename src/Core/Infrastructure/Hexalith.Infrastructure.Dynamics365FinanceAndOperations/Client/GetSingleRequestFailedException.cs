// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

[Serializable]
public sealed class GetSingleRequestFailedException<T> : Exception
{
	public GetSingleRequestFailedException()
	{
	}

	public GetSingleRequestFailedException(string? message) : base(message)
	{
	}

	public GetSingleRequestFailedException(string? message, Exception? innerException) : base(message, innerException)
	{
	}

	public GetSingleRequestFailedException(string entityName, Dictionary<string, object> keys, string? responseContent, Exception? ex)
		: base($"Failed to retrieve {typeof(T).Name} with keys {keys} on entity {entityName}.", ex)
	{
		EntityName = entityName;
		Keys = keys;
		ResponseContent = responseContent;
	}

	private GetSingleRequestFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}

	public string? EntityName { get; private set; }

	public Dictionary<string, object>? Keys { get; private set; }

	public string? ResponseContent { get; private set; }

	public override void GetObjectData(SerializationInfo info, StreamingContext context) => base.GetObjectData(info, context);
}