// <copyright file="PatchRequestFailedException.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;

using System;
using System.Runtime.Serialization;
using System.Text.Json;

/// <summary>
/// Exception thrown when a patch request failed.
/// </summary>
[Serializable]
public class PatchRequestFailedException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PatchRequestFailedException"/> class.
    /// </summary>
    public PatchRequestFailedException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PatchRequestFailedException"/> class.
    /// </summary>
    /// <param name="message">Error message.</param>
    public PatchRequestFailedException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PatchRequestFailedException"/> class.
    /// </summary>
    /// <param name="message">Error message.</param>
    /// <param name="innerException">Inner exception.</param>
    public PatchRequestFailedException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PatchRequestFailedException"/> class.
    /// </summary>
    /// <param name="entityName">The pached entity name.</param>
    /// <param name="url">The patch URL.</param>
    /// <param name="value">The patch value.</param>
    /// <param name="responseContent">The response content.</param>
    /// <param name="message">Error message.</param>
    /// <param name="innerException">Inner exception.</param>
    public PatchRequestFailedException(string entityName, Uri url, object? value, string? responseContent, string? message, Exception? innerException)
        : base($"Failed to patch entity {entityName} at {url?.AbsolutePath}. " + message, innerException)
    {
        EntityName = entityName;
        Url = url;
        Value = JsonSerializer.Serialize(value);
        ValueType = value?.GetType().Name;
        ResponseContent = responseContent;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PatchRequestFailedException"/> class.
    /// </summary>
    /// <param name="info">Serialization information.</param>
    /// <param name="context">Streaming context.</param>
    protected PatchRequestFailedException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    /// <summary>
    /// Gets the patched entity name.
    /// </summary>
    public string? EntityName { get; }

    /// <summary>
    /// Gets patch response content.
    /// </summary>
    public string? ResponseContent { get; }

    /// <summary>
    /// Gets patch URL.
    /// </summary>
    public Uri? Url { get; }

    /// <summary>
    /// Gets patched value.
    /// </summary>
    public string? Value { get; }

    /// <summary>
    /// Gets patched value type name.
    /// </summary>
    public string? ValueType { get; }

    /// <summary>
    /// Get object data for serialization.
    /// </summary>
    /// <param name="info">Serialization information.</param>
    /// <param name="context">Streaming context.</param>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
    }
}
