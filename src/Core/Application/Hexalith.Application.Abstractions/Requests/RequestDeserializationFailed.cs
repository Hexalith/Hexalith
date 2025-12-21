// <copyright file="RequestDeserializationFailed.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

using System.Runtime.Serialization;
using System.Text.Json;

using Hexalith.Commons.Errors;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;

/// <summary>
/// Class RequestDeserializationFailed.
/// Implements the <see cref="ApplicationError" />
/// Implements the <see cref="IEquatable{Error}" />
/// Implements the <see cref="IEquatable{RequestDeserializationFailed}" />.
/// </summary>
/// <seealso cref="ApplicationError" />
/// <seealso cref="IEquatable{Error}" />
/// <seealso cref="IEquatable{RequestDeserializationFailed}" />
[DataContract]
public record RequestDeserializationFailed : ApplicationError
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequestDeserializationFailed" /> class.
    /// </summary>
    /// <param name="data">The serialization data.</param>
    /// <param name="exception">The serialization exception.</param>
    /// <exception cref="System.ArgumentNullException">Exception object is null.</exception>
    public RequestDeserializationFailed(
        string? data,
        SerializationException exception)
    {
        ArgumentNullException.ThrowIfNull(exception);
        _ = exception;
        Title = "Request deserialization failed";
        Type = nameof(RequestDeserializationFailed);
        Detail = "Could not deserialize data : {Message}";
        Arguments = [exception.Message];
        TechnicalDetail = "Request deserialization failed for data:\n{SerializationData}\n{ErrorMessage}\n{StackTrace}";
        TechnicalArguments = [data ?? string.Empty, exception.FullMessage(), exception.StackTrace ?? string.Empty];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestDeserializationFailed" /> class.
    /// </summary>
    /// <param name="data">The serialization data.</param>
    /// <param name="exception">The serialization exception.</param>
    /// <exception cref="System.ArgumentNullException">Exception object is null.</exception>
    public RequestDeserializationFailed(
        string? data,
        JsonException exception)
    {
        ArgumentNullException.ThrowIfNull(exception);
        _ = exception;
        Title = "Request deserialization failed";
        Type = nameof(RequestDeserializationFailed);
        Detail = "Could not deserialize data : {Message}";
        Arguments = [exception.Message];
        TechnicalDetail = "Request deserialization failed for data:\n{SerializationData}\n{ErrorMessage}\n{StackTrace}";
        TechnicalArguments = [data ?? string.Empty, exception.FullMessage(), exception.StackTrace ?? string.Empty];
    }
}