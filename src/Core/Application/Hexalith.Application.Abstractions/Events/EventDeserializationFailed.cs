// <copyright file="EventDeserializationFailed.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Events;

using System.Runtime.Serialization;

using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;

/// <summary>
/// Class EventDeserializationFailed.
/// Implements the <see cref="Error" />
/// Implements the <see cref="IEquatable{Error}" />
/// Implements the <see cref="IEquatable{EventDeserializationFailed}" />.
/// </summary>
/// <seealso cref="Error" />
/// <seealso cref="IEquatable{Error}" />
/// <seealso cref="IEquatable{EventDeserializationFailed}" />
[DataContract]
public record EventDeserializationFailed : Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventDeserializationFailed"/> class.
    /// </summary>
    /// <param name="data">The serialization data.</param>
    /// <param name="exception">The serialization exception.</param>
    public EventDeserializationFailed(
        string? data,
        SerializationException exception)
    {
        ArgumentNullException.ThrowIfNull(exception);
        _ = exception;
        Title = "Event deserialization failed";
        Type = nameof(EventDeserializationFailed);
        Detail = "Could not deserialize data : {Message}";
        Arguments = new object[] { exception.Message };
        TechnicalDetail = "Event deserialization failed for data:\n{SerializationData}\n{ErrorMessage}\n{StackTrace}";
        TechnicalArguments = new object[] { data ?? string.Empty, exception.FullMessage(), exception.StackTrace ?? string.Empty };
    }
}