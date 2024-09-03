// ***********************************************************************
// Assembly         : Hexalith.Domain.Abstractions
// Author           : Jérôme Piquot
// Created          : 04-28-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-30-2023
// ***********************************************************************
// <copyright file="InvalidCommandAggregateIdException.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Exceptions;

using System;

using Hexalith.Application.Commands;

/// <summary>
/// Class InvalidAggregateCommandException.
/// Implements the <see cref="InvalidOperationException" />.
/// </summary>
/// <seealso cref="InvalidOperationException" />
public class InvalidCommandAggregateIdException : InvalidOperationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidCommandAggregateIdException" /> class.
    /// </summary>
    public InvalidCommandAggregateIdException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidCommandAggregateIdException" /> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public InvalidCommandAggregateIdException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidCommandAggregateIdException" /> class.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="command">The domain command.</param>
    public InvalidCommandAggregateIdException(string aggregateId, BaseCommand command)
        : base($"Command '{command?.TypeName ?? "Unknown"}' has an invalid aggregate identifier '{command?.AggregateId}'. Expected : {aggregateId}.")
    {
        AggregateId = aggregateId;
        DomainCommand = command;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidCommandAggregateIdException" /> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not a null reference (<see langword="Nothing" /> in Visual Basic), the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
    public InvalidCommandAggregateIdException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Gets the aggregate.
    /// </summary>
    /// <value>The aggregate.</value>
    public string? AggregateId { get; }

    /// <summary>
    /// Gets the domain command.
    /// </summary>
    /// <value>The domain command.</value>
    public BaseCommand? DomainCommand { get; }
}