// <copyright file="GetSingleRequestFailedException.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

/*
 * <Your-Product-Name>
 * Copyright (c) <Year-From>-<Year-To> <Your-Company-Name>
 *
 * Please configure this header in your SonarCloud/SonarQube quality profile.
 * You can also set it in SonarLint.xml additional file for SonarLint or standalone NuGet analyzer.
 */

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;

using System.Runtime.Serialization;

/// <summary>
/// Exception thrown when a single request failed.
/// </summary>
/// <typeparam name="T">Type of the returned entity.</typeparam>
[DataContract]
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
    /// <param name="message">The error message.</param>
    public GetSingleRequestFailedException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSingleRequestFailedException{T}"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public GetSingleRequestFailedException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSingleRequestFailedException{T}"/> class.
    /// </summary>
    /// <param name="entityName">The entity name.</param>
    /// <param name="keys">The keys used to get the entity.</param>
    /// <param name="responseContent">The returned response.</param>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public GetSingleRequestFailedException(string entityName, IDictionary<string, object?> keys, string? responseContent, string? message, Exception? innerException)
        : base($"Failed to retrieve {typeof(T).Name} with keys {keys} on entity {entityName}. " + message, innerException)
    {
        EntityName = entityName;
        Keys = keys;
        ResponseContent = responseContent;
    }

    private GetSingleRequestFailedException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    /// <summary>
    /// Gets the entity name.
    /// </summary>
    /// <value>
    /// The entity name.
    /// </value>
    public string? EntityName { get; private set; }

    /// <summary>
    /// Gets the keys used to get the entity.
    /// </summary>
    /// <value>
    /// The keys used to get the entity.
    /// </value>
    public IDictionary<string, object?>? Keys { get; private set; }

    /// <summary>
    /// Gets the returned response.
    /// </summary>
    /// <value>
    /// The returned response.
    /// </value>
    public string? ResponseContent { get; private set; }

    /// <inheritdoc/>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
    }
}