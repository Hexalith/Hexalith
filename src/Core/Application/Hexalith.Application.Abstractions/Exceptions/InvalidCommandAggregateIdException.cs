// <copyright file="InvalidCommandDomainIdException.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Exceptions;

using System;
using System.Text.Json;

using Hexalith.Commons.Metadatas;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents an exception that is thrown when a command is associated with an invalid aggregate identifier.
/// This exception is used in domain-driven design contexts to ensure command-aggregate integrity.
/// </summary>
/// <seealso cref="InvalidOperationException" />
public class InvalidCommandDomainIdException : InvalidOperationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidCommandDomainIdException" /> class.
    /// </summary>
    public InvalidCommandDomainIdException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidCommandDomainIdException" /> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public InvalidCommandDomainIdException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidCommandDomainIdException"/> class with detailed information about the invalid command.
    /// </summary>
    /// <param name="expectedPartitionKey">The expected partition key.</param>
    /// <param name="command">The command object that caused the exception.</param>
    /// <param name="metadata">The metadata associated with the command, containing information about the message and aggregate.</param>
    public InvalidCommandDomainIdException(string expectedPartitionKey, object command, Metadata metadata)
        : base($"Command '{metadata?.Message.Name ?? "Unknown"}' has an invalid partition key '{metadata?.DomainGlobalId}'. Expected : {expectedPartitionKey}.")
    {
        ExpectedPartitionKey = expectedPartitionKey;
        Command = JsonSerializer.Serialize(command, PolymorphicHelper.DefaultJsonSerializerOptions);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidCommandDomainIdException" /> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference
    /// (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.
    /// </param>
    public InvalidCommandDomainIdException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Gets the serialized representation of the domain command that caused the exception.
    /// </summary>
    /// <value>A JSON string representation of the command object.</value>
    public string? Command { get; }

    /// <summary>
    /// Gets the expected aggregate identifier that should have been associated with the command.
    /// </summary>
    /// <value>The expected aggregate identifier.</value>
    public string? ExpectedPartitionKey { get; }
}