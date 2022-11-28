// <copyright file="UnexpectedStreamVersionException.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.StreamStores;

using System.Runtime.Serialization;

[Serializable]
public class UnexpectedStreamVersionException : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="UnexpectedStreamVersionException"/> class.
	/// </summary>
	public UnexpectedStreamVersionException()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="UnexpectedStreamVersionException"/> class.
	/// </summary>
	/// <param name="message"></param>
	public UnexpectedStreamVersionException(string? message)
		: base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="UnexpectedStreamVersionException"/> class.
	/// </summary>
	/// <param name="expectedVersion"></param>
	/// <param name="actualVersion"></param>
	/// <param name="message"></param>
	/// <param name="innerException"></param>
	public UnexpectedStreamVersionException(
	long expectedVersion,
	long actualVersion,
	string? message,
	Exception? innerException)
	: base($"Unexpected stream version '{expectedVersion}'. Actual version : '{actualVersion}'. " + message, innerException)
	{
		ExpectedVersion = expectedVersion;
		ActualVersion = actualVersion;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="UnexpectedStreamVersionException"/> class.
	/// </summary>
	/// <param name="expectedVersion"></param>
	/// <param name="actualVersion"></param>
	public UnexpectedStreamVersionException(
	long expectedVersion,
	long actualVersion)
	: this(expectedVersion, actualVersion, message: null, innerException: null)
	{
		ExpectedVersion = expectedVersion;
		ActualVersion = actualVersion;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="UnexpectedStreamVersionException"/> class.
	/// </summary>
	/// <param name="message"></param>
	/// <param name="innerException"></param>
	public UnexpectedStreamVersionException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="UnexpectedStreamVersionException"/> class.
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	protected UnexpectedStreamVersionException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public long ActualVersion { get; private set; }

	public long ExpectedVersion { get; private set; }
}