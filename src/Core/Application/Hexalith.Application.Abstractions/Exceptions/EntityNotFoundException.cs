// <copyright file="EntityNotFoundException.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Exceptions;

using System;
using System.Runtime.Serialization;
using System.Text.Json;

/// <summary>
/// Class EntityNotFoundException.
/// Implements the <see cref="Exception" />.
/// </summary>
/// <typeparam name="TEntity">The type of the t entity.</typeparam>
/// <seealso cref="Exception" />
[Serializable]
public class EntityNotFoundException<TEntity> : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException{TEntity}"/> class.
    /// </summary>
    public EntityNotFoundException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException{TEntity}"/> class.
    /// </summary>
    /// <param name="key">The key.</param>
    public EntityNotFoundException(object key)
       : base($"Entity '{typeof(TEntity).Name}' not found for key: {JsonSerializer.Serialize(key)}")
    {
        Key = key;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException{TEntity}"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public EntityNotFoundException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException{TEntity}"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">
    /// The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.
    /// </param>
    public EntityNotFoundException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException{TEntity}"/> class.
    /// </summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
    protected EntityNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    /// <summary>
    /// Gets the key.
    /// </summary>
    /// <value>The key.</value>
    public object? Key { get; }
}
