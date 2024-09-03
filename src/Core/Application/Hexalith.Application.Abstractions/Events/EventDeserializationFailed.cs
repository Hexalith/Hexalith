// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 08-09-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-07-2023
// ***********************************************************************
// <copyright file="EventDeserializationFailed.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Events;

using System.Runtime.Serialization;
using System.Text.Json;

using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;

/// <summary>
/// Class EventDeserializationFailed.
/// Implements the <see cref="ApplicationError" />
/// Implements the <see cref="IEquatable{Error}" />
/// Implements the <see cref="IEquatable{EventDeserializationFailed}" />.
/// </summary>
/// <seealso cref="ApplicationError" />
/// <seealso cref="IEquatable{Error}" />
/// <seealso cref="IEquatable{EventDeserializationFailed}" />
[DataContract]
public record EventDeserializationFailed : ApplicationError
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventDeserializationFailed" /> class.
    /// </summary>
    /// <param name="data">The serialization data.</param>
    /// <param name="exception">The serialization exception.</param>
    /// <exception cref="System.ArgumentNullException">Exception object is null.</exception>
    public EventDeserializationFailed(
        string? data,
        SerializationException exception)
    {
        ArgumentNullException.ThrowIfNull(exception);
        _ = exception;
        Title = "Event deserialization failed";
        Type = nameof(EventDeserializationFailed);
        Detail = "Could not deserialize data : {Message}";
        Arguments = [exception.Message];
        TechnicalDetail = "Event deserialization failed for data:\n{SerializationData}\n{ErrorMessage}\n{StackTrace}";
        TechnicalArguments = [data ?? string.Empty, exception.FullMessage(), exception.StackTrace ?? string.Empty];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventDeserializationFailed" /> class.
    /// </summary>
    /// <param name="data">The serialization data.</param>
    /// <param name="exception">The serialization exception.</param>
    /// <exception cref="System.ArgumentNullException">Exception object is null.</exception>
    public EventDeserializationFailed(
        string? data,
        JsonException exception)
    {
        ArgumentNullException.ThrowIfNull(exception);
        _ = exception;
        Title = "Event deserialization failed";
        Type = nameof(EventDeserializationFailed);
        Detail = "Could not deserialize data : {Message}";
        Arguments = [exception.Message];
        TechnicalDetail = "Event deserialization failed for data:\n{SerializationData}\n{ErrorMessage}\n{StackTrace}";
        TechnicalArguments = [data ?? string.Empty, exception.FullMessage(), exception.StackTrace ?? string.Empty];
    }
}