// <copyright file="EventValidationFailed.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Events;

using System.Runtime.Serialization;

using Hexalith.Commons.Errors;

/// <summary>
/// Class EventValidationFailed.
/// Implements the <see cref="ApplicationError" />
/// Implements the <see cref="IEquatable{Error}" />
/// Implements the <see cref="IEquatable{EventValidationFailed}" />.
/// </summary>
/// <seealso cref="ApplicationError" />
/// <seealso cref="IEquatable{Error}" />
/// <seealso cref="IEquatable{EventValidationFailed}" />
[DataContract]
public record EventValidationFailed : ApplicationError
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventValidationFailed"/> class.
    /// </summary>
    /// <param name="data">The serialization data.</param>
    /// <param name="exception">The serialization exception.</param>
    public EventValidationFailed(
        string data,
        Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);
        Title = "Event validation failed";
        Type = nameof(EventValidationFailed);
        Detail = "Could not validate event : {Message}";
        Arguments = [exception.Message];
        TechnicalDetail = "Event validation failed for data:\n{SerializationData}\n{ErrorMessage}\n{StackTrace}";
        TechnicalArguments = [data, exception.FullMessage(), exception.StackTrace ?? string.Empty];
    }
}