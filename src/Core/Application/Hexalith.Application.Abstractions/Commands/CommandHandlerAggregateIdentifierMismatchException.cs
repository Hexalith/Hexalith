// <copyright file="CommandHandlerAggregateIdentifierMismatchException.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Commands;

using System;

using Hexalith.Application.Metadatas;

/// <summary>
/// Exception thrown when the aggregate identifier does not match the expected identifier.
/// </summary>
/// <param name="expectedAggregateId">The expected identifier.</param>
/// <param name="metadata">The metadata associated with the command.</param>
/// <param name="message">The error message.</param>
/// <param name="exception">The inner exception.</param>
public class CommandHandlerAggregateIdentifierMismatchException(string? expectedAggregateId, Metadata? metadata, string? message, Exception? exception)
        : Exception(
            (string.IsNullOrWhiteSpace(expectedAggregateId)
                ? string.Empty
                : $"Could not handle command because the aggregate identifier does not match the expected identifier '{expectedAggregateId}'. ") +
            (string.IsNullOrWhiteSpace(message) ? string.Empty : message + "\n") +
            (metadata is null ? string.Empty : metadata.ToLogString()),
            exception)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandHandlerAggregateIdentifierMismatchException"/> class.
    /// </summary>
    public CommandHandlerAggregateIdentifierMismatchException()
        : this(null, null, null, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandHandlerAggregateIdentifierMismatchException"/> class with the specified expected aggregate identifier and metadata.
    /// </summary>
    /// <param name="expectedAggregateId">The expected identifier.</param>
    /// <param name="metadata">The metadata associated with the command.</param>
    public CommandHandlerAggregateIdentifierMismatchException(string expectedAggregateId, Metadata metadata)
        : this(expectedAggregateId, metadata, null, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandHandlerAggregateIdentifierMismatchException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public CommandHandlerAggregateIdentifierMismatchException(string message)
        : this(null, null, message, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandHandlerAggregateIdentifierMismatchException"/>
    /// class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="exception">The inner exception.</param>
    public CommandHandlerAggregateIdentifierMismatchException(string message, Exception exception)
        : this(null, null, message, exception)
    {
    }

    /// <summary>
    /// Gets the expected aggregate identifier.
    /// </summary>
    public string? ExpectedAggregateId { get; } = expectedAggregateId;

    /// <summary>
    /// Gets the metadata associated with the command.
    /// </summary>
    public Metadata? Metadata { get; } = metadata;
}