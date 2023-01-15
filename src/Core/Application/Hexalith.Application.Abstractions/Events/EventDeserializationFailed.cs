// <copyright file="EventDeserializationFailed.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Events;

using Ardalis.GuardClauses;

using Hexalith.Application.Abstractions.Errors;
using Hexalith.Extensions.Helpers;

using System.Runtime.Serialization;

/// <summary>
/// Class EventDeserializationFailed.
/// Implements the <see cref="Error" />
/// Implements the <see cref="System.IEquatable{Hexalith.Application.Abstractions.Errors.Error}" />
/// Implements the <see cref="System.IEquatable{Hexalith.Application.Abstractions.Events.EventDeserializationFailed}" />.
/// </summary>
/// <seealso cref="Error" />
/// <seealso cref="System.IEquatable{Hexalith.Application.Abstractions.Errors.Error}" />
/// <seealso cref="System.IEquatable{Hexalith.Application.Abstractions.Events.EventDeserializationFailed}" />
[DataContract]
public record EventDeserializationFailed : Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventDeserializationFailed"/> class.
    /// </summary>
    /// <param name="data">The serialization data.</param>
    /// <param name="exception">The serialization exception.</param>
    public EventDeserializationFailed(
        string data,
        SerializationException exception)
    {
        _ = Guard.Against.Null(exception);
        Title = "Event deserialization failed";
        Type = nameof(EventDeserializationFailed);
        Detail = "Could not deserialize data : {Message}";
        Arguments = new object[] { exception.Message };
        TechnicalDetail = "Event deserialization failed for data:\n{SerializationData}\n{ErrorMessage}\n{StackTrace}";
        TechnicalArguments = new object[] { data, exception.FullMessage(), exception.StackTrace ?? string.Empty };
    }
}