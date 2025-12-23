// <copyright file="EntityNotFoundException.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Exceptions;

using System;
using System.Runtime.Serialization;
using System.Text.Json;

using Hexalith.Commons.Errors;

/// <summary>
/// Class EntityNotFoundException.
/// Implements the <see cref="Exception" />.
/// </summary>
/// <typeparam name="TEntity">The type of the t entity.</typeparam>
/// <seealso cref="Exception" />
[DataContract]
public class EntityNotFoundException<TEntity> : ApplicationErrorException
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
       : base(CreateError(key))
    {
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
    /// Gets the key.
    /// </summary>
    /// <value>The key.</value>
    public object? Key { get; }

    private static ApplicationError CreateError(object key)
    {
        string entityName = typeof(TEntity).Name;
#pragma warning disable CA1869 // Cache and reuse 'JsonSerializerOptions' instances
        _ = JsonSerializer.Serialize(key, new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault });
#pragma warning restore CA1869 // Cache and reuse 'JsonSerializerOptions' instances
        return new()
        {
            Title = entityName + " not found.",
            Detail = "Entity {EntityName} not found with key:\n{Key}",
            Arguments = [entityName, key],
            Category = Commons.Errors.ErrorCategory.Functional,
        };
    }
}