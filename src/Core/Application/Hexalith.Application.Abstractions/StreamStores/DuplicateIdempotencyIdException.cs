// <copyright file="DuplicateIdempotencyIdException.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.StreamStores;

using System;
using System.Runtime.Serialization;

/// <summary>
/// Class DuplicateIdempotencyIdException.
/// Implements the <see cref="Exception" />.
/// </summary>
/// <seealso cref="Exception" />
[DataContract]
public class DuplicateIdempotencyIdException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateIdempotencyIdException" /> class.
    /// </summary>
    public DuplicateIdempotencyIdException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateIdempotencyIdException" /> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public DuplicateIdempotencyIdException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateIdempotencyIdException"/> class.
    /// </summary>
    /// <param name="idempotencyId">The idempotency identifier.</param>
    /// <param name="duplicateData">The duplicate data.</param>
    public DuplicateIdempotencyIdException(string idempotencyId, object duplicateData)
    {
        IdempotencyId = idempotencyId;
        DuplicateData = duplicateData;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateIdempotencyIdException" /> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception,
    /// or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
    public DuplicateIdempotencyIdException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Gets the duplicate data.
    /// </summary>
    /// <value>The duplicate data.</value>
    public object? DuplicateData { get; private set; }

    /// <summary>
    /// Gets the idempotency identifier.
    /// </summary>
    /// <value>The idempotency identifier.</value>
    public string? IdempotencyId { get; private set; }
}