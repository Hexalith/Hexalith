// <copyright file="InvalidCommandAggregateIdException.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Exceptions;

using System;
using System.Text.Json;

using Hexalith.Application.Metadatas;
using Hexalith.PolymorphicSerialization;

/// <summary>
/// Represents an exception that is thrown when a command is associated with an invalid aggregate identifier.
/// This exception is used in domain-driven design contexts to ensure command-aggregate integrity.
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
    /// Initializes a new instance of the <see cref="InvalidCommandAggregateIdException"/> class with detailed information about the invalid command.
    /// </summary>
    /// <param name="aggregateId">The expected aggregate identifier.</param>
    /// <param name="command">The command object that caused the exception.</param>
    /// <param name="metadata">The metadata associated with the command, containing information about the message and aggregate.</param>
    public InvalidCommandAggregateIdException(string aggregateId, object command, Metadata metadata)
        : base($"Command '{metadata?.Message.Name ?? "Unknown"}' has an invalid aggregate identifier '{metadata?.Message.Aggregate.Id}'. Expected : {aggregateId}.")
    {
        AggregateId = aggregateId;
        Command = JsonSerializer.Serialize(command, PolymorphicHelper.DefaultJsonSerializerOptions);
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
    /// Gets the expected aggregate identifier that should have been associated with the command.
    /// </summary>
    /// <value>The expected aggregate identifier.</value>
    public string? AggregateId { get; }

    /// <summary>
    /// Gets the serialized representation of the domain command that caused the exception.
    /// </summary>
    /// <value>A JSON string representation of the command object.</value>
    public string? Command { get; }
}