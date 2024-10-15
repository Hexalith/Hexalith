// <copyright file="InvalidStatusChangeException.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Tasks;

using System;
using System.Runtime.Serialization;

/// <summary>
/// The exception that is thrown when a task processor status change is invalid.
/// </summary>
[DataContract]
public sealed class InvalidStatusChangeException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidStatusChangeException"/> class.
    /// </summary>
    public InvalidStatusChangeException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidStatusChangeException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public InvalidStatusChangeException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidStatusChangeException"/> class.
    /// </summary>
    /// <param name="currentStatus">The current processor status.</param>
    /// <param name="newStatus">The expected new status.</param>
    /// <param name="message">The error message.</param>
    public InvalidStatusChangeException(TaskProcessorStatus currentStatus, TaskProcessorStatus newStatus, string message)
        : this($"The processor with current status {currentStatus} cannot be changed to {newStatus}. " + message)
    {
        CurrentStatus = currentStatus;
        NewStatus = newStatus;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidStatusChangeException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public InvalidStatusChangeException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Gets the current processor status.
    /// </summary>
    public TaskProcessorStatus? CurrentStatus { get; private set; }

    /// <summary>
    /// Gets the expected new status.
    /// </summary>
    public TaskProcessorStatus? NewStatus { get; private set; }
}