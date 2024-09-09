// <copyright file="InvalidAggregateCommandException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Infrastructure.ClientApp.Services;

using System;
using System.Text.Json;

/// <summary>
/// Represents an exception that is thrown when an invalid aggregate command is encountered.
/// </summary>
[Serializable]
public class InvalidAggregateCommandException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidAggregateCommandException"/> class.
    /// </summary>
    public InvalidAggregateCommandException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidAggregateCommandException"/> class with a specified command and message.
    /// </summary>
    /// <param name="command">The invalid command.</param>
    /// <param name="message">The error message.</param>
    public InvalidAggregateCommandException(object command, string? message)
        : base((message ?? string.Empty) + JsonSerializer.Serialize(command))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidAggregateCommandException"/> class with a specified message and inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public InvalidAggregateCommandException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidAggregateCommandException"/> class with a specified message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public InvalidAggregateCommandException(string message)
        : base(message)
    {
    }
}