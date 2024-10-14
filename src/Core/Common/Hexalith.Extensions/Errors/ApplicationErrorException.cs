// <copyright file="ApplicationErrorException.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Errors;

using System;
using System.Globalization;
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
    public ApplicationErrorException(ApplicationError error)
        : base((error ?? throw new ArgumentNullException(nameof(error))).Title + Environment.NewLine + error.GetDetailMessage(CultureInfo.InvariantCulture)) => Error = error;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationErrorException" /> class.
    /// </summary>
    /// <param name="error">The error.</param>
    /// <param name="innerException">The inner exception.</param>
    public ApplicationErrorException(ApplicationError error, Exception? innerException)
        : base((error ?? throw new ArgumentNullException(nameof(error))).Title + Environment.NewLine + error.GetDetailMessage(CultureInfo.InvariantCulture), innerException) => Error = error;

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
    /// Gets the error.
    /// </summary>
    /// <value>The error.</value>
    public ApplicationError? Error { get; private set; }
}