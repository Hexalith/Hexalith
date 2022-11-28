// <copyright file="GetSingleRequestFailedException.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;

using System.Runtime.Serialization;

[Serializable]
public sealed class GetSingleRequestFailedException<T> : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="GetSingleRequestFailedException{T}"/> class.
	/// </summary>
	public GetSingleRequestFailedException()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="GetSingleRequestFailedException{T}"/> class.
	/// </summary>
	/// <param name="message"></param>
	public GetSingleRequestFailedException(string? message)
		: base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="GetSingleRequestFailedException{T}"/> class.
	/// </summary>
	/// <param name="message"></param>
	/// <param name="innerException"></param>
	public GetSingleRequestFailedException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="GetSingleRequestFailedException{T}"/> class.
	/// </summary>
	/// <param name="entityName"></param>
	/// <param name="keys"></param>
	/// <param name="responseContent"></param>
	/// <param name="ex"></param>
	public GetSingleRequestFailedException(string entityName, Dictionary<string, object> keys, string? responseContent, Exception? ex)
		: base($"Failed to retrieve {typeof(T).Name} with keys {keys} on entity {entityName}.", ex)
	{
		EntityName = entityName;
		Keys = keys;
		ResponseContent = responseContent;
	}

	private GetSingleRequestFailedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public string? EntityName { get; private set; }

	public Dictionary<string, object>? Keys { get; private set; }

	public string? ResponseContent { get; private set; }

	/// <inheritdoc/>
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
	}
}