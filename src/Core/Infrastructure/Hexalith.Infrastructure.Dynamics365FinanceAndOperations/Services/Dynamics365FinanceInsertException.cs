// <copyright file="Dynamics365FinanceInsertException.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Services;

using System;
using System.Runtime.Serialization;
using System.Text.Json;

/// <summary>
/// Class Dynamics365FinanceInsertException.
/// Implements the <see cref="Exception" />.
/// </summary>
/// <typeparam name="TEntity">The type of the t entity.</typeparam>
/// <typeparam name="TCreate">The type of the t create.</typeparam>
/// <seealso cref="Exception" />
[Serializable]
public class Dynamics365FinanceInsertException<TEntity, TCreate> : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceInsertException{TEntity, TCreate}" /> class.
    /// </summary>
    public Dynamics365FinanceInsertException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceInsertException{TEntity, TCreate}" /> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public Dynamics365FinanceInsertException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceInsertException{TEntity, TCreate}" /> class.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="innerException">The inner exception.</param>
    public Dynamics365FinanceInsertException(TCreate? entity, Exception innerException)
        : base(
        $"Error while inserting {typeof(TEntity)} in Dynamics 365 for finance dans operations.",
        innerException)
    {
        Data = JsonSerializer.Serialize(entity);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceInsertException{TEntity, TCreate}" /> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
    public Dynamics365FinanceInsertException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceInsertException{TEntity, TCreate}" /> class.
    /// </summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
    protected Dynamics365FinanceInsertException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    /// <summary>
    /// Gets a collection of key/value pairs that provide additional user-defined information about the exception.
    /// </summary>
    /// <value>The data.</value>
    public string? Data { get; }
}
