// <copyright file="ApplicationErrorException.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Errors;

using System;
using System.Runtime.Serialization;

using Hexalith.Extensions.Common;

/// <summary>
/// Class ApplicationErrorException.
/// Implements the <see cref="Exception" />.
/// </summary>
/// <seealso cref="Exception" />
[DataContract]
public class ApplicationErrorException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationErrorException" /> class.
    /// </summary>
    public ApplicationErrorException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationErrorException" /> class.
    /// </summary>
    /// <param name="error">The error details.</param>
    public ApplicationErrorException(Error error)
        : base(error.Title + Environment.NewLine + error.Detail)
    {
        Error = error;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationErrorException"/> class.
    /// </summary>
    /// <param name="error">The error.</param>
    /// <param name="innerException">The inner exception.</param>
    public ApplicationErrorException(Error error, Exception? innerException)
        : base(error.Title + Environment.NewLine + error.Detail, innerException)
    {
        Error = error;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationErrorException" /> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ApplicationErrorException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationErrorException" /> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
    public ApplicationErrorException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationErrorException" /> class.
    /// </summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
    protected ApplicationErrorException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    /// <summary>
    /// Gets the error.
    /// </summary>
    /// <value>The error.</value>
    public Error? Error { get; private set; }
}