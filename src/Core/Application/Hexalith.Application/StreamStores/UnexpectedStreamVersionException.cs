// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Application.StreamStores;

using System;
using System.Runtime.Serialization;

[Serializable]
public class UnexpectedStreamVersionException : Exception
{
	public UnexpectedStreamVersionException()
	{
	}

	public UnexpectedStreamVersionException(string? message) : base(message)
	{
	}

	public UnexpectedStreamVersionException(
		long expectedVersion,
		long actualVersion,
		string? message = null,
		Exception? innerException = null)
		: base($"Unexpected stream version '{expectedVersion}'. Actual version : '{actualVersion}'. " + message, innerException)
	{
		ExpectedVersion = expectedVersion;
		ActualVersion = actualVersion;
	}

	public UnexpectedStreamVersionException(string? message, Exception? innerException) : base(message, innerException)
	{
	}

	protected UnexpectedStreamVersionException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}

	public long ActualVersion { get; private set; }
	public long ExpectedVersion { get; private set; }
}