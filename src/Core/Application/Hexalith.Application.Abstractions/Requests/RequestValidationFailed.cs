// <copyright file="RequestValidationFailed.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

using System.Runtime.Serialization;

using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;

/// <summary>
/// Class RequestValidationFailed.
/// Implements the <see cref="ApplicationError" />
/// Implements the <see cref="IEquatable{Error}" />
/// Implements the <see cref="IEquatable{RequestValidationFailed}" />.
/// </summary>
/// <seealso cref="ApplicationError" />
/// <seealso cref="IEquatable{Error}" />
/// <seealso cref="IEquatable{RequestValidationFailed}" />
[DataContract]
public record RequestValidationFailed : ApplicationError
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequestValidationFailed"/> class.
    /// </summary>
    /// <param name="data">The serialization data.</param>
    /// <param name="exception">The serialization exception.</param>
    public RequestValidationFailed(
        string data,
        Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);
        Title = "Request validation failed";
        Type = nameof(RequestValidationFailed);
        Detail = "Could not validate request : {Message}";
        Arguments = [exception.Message];
        TechnicalDetail = "Request validation failed for data:\n{SerializationData}\n{ErrorMessage}\n{StackTrace}";
        TechnicalArguments = [data, exception.FullMessage(), exception.StackTrace ?? string.Empty];
    }
}