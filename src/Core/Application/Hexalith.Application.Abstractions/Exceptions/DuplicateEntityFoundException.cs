// <copyright file="DuplicateEntityFoundException.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Exceptions;

using System;
using System.Runtime.Serialization;
using System.Text.Json;

using Hexalith.Extensions.Common;
using Hexalith.Extensions.Errors;

/// <summary>
/// Class DuplicateEntityFoundException.
/// Implements the <see cref="Exception" />.
/// </summary>
/// <typeparam name="TEntity">The type of the t entity.</typeparam>
/// <seealso cref="Exception" />
[DataContract]
public class DuplicateEntityFoundException<TEntity> : ApplicationErrorException
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
    public DuplicateEntityFoundException(object key, IEnumerable<TEntity> duplicates)
        : base(CreateError(key, duplicates)) => Key = key;

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
    /// Gets the key.
    /// </summary>
    /// <value>The key.</value>
    public object? Key { get; private set; }

    private static ApplicationError CreateError(object key, IEnumerable<TEntity> duplicates)
    {
        JsonSerializerOptions o = new() { WriteIndented = true, DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault };
        string entityName = typeof(TEntity).Name;
        string keyString = JsonSerializer.Serialize(key, o);
        string valuesString = string.Join('\n', duplicates.Select(p => JsonSerializer.Serialize(p, o)));
        return new ApplicationError
        {
            Title = $"Duplicate {entityName} found.",
            Detail = "Duplicate {EntityName} values found for key {Key}. Values:\n{Values}",
            Arguments = new[] { entityName, keyString, valuesString },
            Category = ErrorCategory.Functional,
        };
    }
}