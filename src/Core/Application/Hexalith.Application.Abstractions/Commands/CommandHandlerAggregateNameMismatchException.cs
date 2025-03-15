// <copyright file="CommandHandlerAggregateNameMismatchException.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Commands;

using System;

using Hexalith.Application.Metadatas;

/// <summary>
/// Exception thrown when there is a mismatch between the expected aggregate name and the actual aggregate name in the command handler.
/// </summary>
/// <param name="expectedAggregateName">The expected aggregate name.</param>
/// <param name="metadata">The metadata associated with the command.</param>
/// <param name="message">The error message.</param>
/// <param name="exception">The inner exception.</param>
public class CommandHandlerAggregateNameMismatchException(string? expectedAggregateName, Metadata? metadata, string? message, Exception? exception)
        : Exception(
            (string.IsNullOrWhiteSpace(expectedAggregateName)
                ? string.Empty
                : $"Could not handle command because the aggregate name does not match the expected name '{expectedAggregateName}'. ") +
            (string.IsNullOrWhiteSpace(message) ? string.Empty : message + "\n") +
            (metadata is null ? string.Empty : metadata.ToLogString()),
            exception)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandHandlerAggregateNameMismatchException"/> class.
    /// </summary>
    public CommandHandlerAggregateNameMismatchException()
        : this(null, null, null, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandHandlerAggregateNameMismatchException"/> class with the specified expected aggregate name and metadata.
    /// </summary>
    /// <param name="expectedAggregateName">The expected aggregate name.</param>
    /// <param name="metadata">The metadata associated with the command.</param>
    public CommandHandlerAggregateNameMismatchException(string expectedAggregateName, Metadata metadata)
        : this(expectedAggregateName, metadata, null, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandHandlerAggregateNameMismatchException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public CommandHandlerAggregateNameMismatchException(string message)
        : this(null, null, message, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandHandlerAggregateNameMismatchException"/>
    /// class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="exception">The inner exception.</param>
    public CommandHandlerAggregateNameMismatchException(string message, Exception exception)
        : this(null, null, message, exception)
    {
    }

    /// <summary>
    /// Gets the expected aggregate name.
    /// </summary>
    public string? ExpectedAggregateName { get; } = expectedAggregateName;

    /// <summary>
    /// Gets the metadata associated with the command.
    /// </summary>
    public Metadata? Metadata { get; } = metadata;
}