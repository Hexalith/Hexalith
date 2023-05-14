// <copyright file="DuplicateEntityFoundException.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Exceptions;

using System;
using System.Runtime.Serialization;
using System.Text.Json;

/// <summary>
/// Class DuplicateEntityFoundException.
/// Implements the <see cref="Exception" />.
/// </summary>
/// <typeparam name="TEntity">The type of the t entity.</typeparam>
/// <seealso cref="Exception" />
[DataContract]
public class DuplicateEntityFoundException<TEntity> : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateEntityFoundException{TEntity}"/> class.
    /// </summary>
    public DuplicateEntityFoundException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateEntityFoundException{TEntity}"/> class.
    /// </summary>
    /// <param name="key">The key.</param>
    public DuplicateEntityFoundException(object key)
        : base($"Duplicate entity '{typeof(TEntity).Name}' found for key: {JsonSerializer.Serialize(key)}")
    {
        Key = key;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateEntityFoundException{TEntity}"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public DuplicateEntityFoundException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateEntityFoundException{TEntity}"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">
    /// The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.
    /// </param>
    public DuplicateEntityFoundException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateEntityFoundException{TEntity}"/> class.
    /// </summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
    protected DuplicateEntityFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    /// <summary>
    /// Gets the key.
    /// </summary>
    /// <value>The key.</value>
    public object? Key { get; private set; }
}