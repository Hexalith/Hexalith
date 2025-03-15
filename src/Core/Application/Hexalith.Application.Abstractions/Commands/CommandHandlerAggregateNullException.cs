// <copyright file="CommandHandlerAggregateNullException.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Commands;

using System;

using Hexalith.Application.Metadatas;

/// <summary>
/// Exception thrown when the aggregate is null in the command handler.
/// </summary>
/// <param name="metadata">The metadata associated with the command.</param>
/// <param name="message">The error message.</param>
/// <param name="exception">The inner exception.</param>
public class CommandHandlerAggregateNullException(Metadata? metadata, string? message, Exception? exception)
    : Exception(
        "Could not handle command because the aggregate is null. " +
        (string.IsNullOrWhiteSpace(message) ? string.Empty : message + "\n") +
        (metadata is null ? string.Empty : metadata.ToLogString()),
        exception)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandHandlerAggregateNullException"/> class.
    /// </summary>
    public CommandHandlerAggregateNullException()
        : this(null, null, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandHandlerAggregateNullException"/> class with the specified metadata.
    /// </summary>
    /// <param name="metadata">The metadata associated with the command.</param>
    public CommandHandlerAggregateNullException(Metadata metadata)
        : this(metadata, null, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandHandlerAggregateNullException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public CommandHandlerAggregateNullException(string message)
        : this(null, message, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandHandlerAggregateNullException"/>
    /// class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="exception">The inner exception.</param>
    public CommandHandlerAggregateNullException(string message, Exception exception)
        : this(null, message, exception)
    {
    }

    /// <summary>
    /// Gets the metadata associated with the command.
    /// </summary>
    public Metadata? Metadata { get; } = metadata;
}