// <copyright file="InvalidAggregateEventException.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Exceptions;

using System;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Errors;

/// <summary>
/// Class InvalidAggregateEventException.
/// Implements the <see cref="InvalidOperationException" />.
/// </summary>
/// <seealso cref="InvalidOperationException" />
public class InvalidAggregateEventException : ApplicationErrorException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidAggregateEventException"/> class.
    /// </summary>
    public InvalidAggregateEventException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidAggregateEventException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public InvalidAggregateEventException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidAggregateEventException"/> class.
    /// </summary>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="isInitializerEvent">if set to <c>true</c> [is initializer event].</param>
    /// <param name="message">The message.</param>
    [Obsolete]
    public InvalidAggregateEventException(IDomainAggregate aggregate, BaseEvent domainEvent, bool isInitializerEvent, string? message)
        : base(GetError(aggregate, domainEvent, isInitializerEvent, message), null)
    {
        Aggregate = aggregate;
        DomainEvent = domainEvent;
        IsInitializerEvent = isInitializerEvent;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidAggregateEventException"/> class.
    /// </summary>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="isInitializerEvent">if set to <c>true</c> [is initializer event].</param>
    [Obsolete]
    public InvalidAggregateEventException(IDomainAggregate aggregate, BaseEvent domainEvent, bool isInitializerEvent)
        : base(GetError(aggregate, domainEvent, isInitializerEvent, null), null)
    {
        Aggregate = aggregate;
        DomainEvent = domainEvent;
        IsInitializerEvent = isInitializerEvent;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidAggregateEventException"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">
    /// The exception that is the cause of the current exception.
    /// If the <paramref name="innerException" /> parameter is not a null reference (<see langword="Nothing" /> in Visual Basic),
    /// the current exception is raised in a <see langword="catch" /> block that handles the inner exception.
    /// </param>
    public InvalidAggregateEventException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Gets the aggregate.
    /// </summary>
    /// <value>The aggregate.</value>
    [Obsolete]
    public IDomainAggregate? Aggregate { get; }

    /// <summary>
    /// Gets the domain event.
    /// </summary>
    /// <value>The domain event.</value>
    [Obsolete]
    public BaseEvent? DomainEvent { get; }

    /// <summary>
    /// Gets a value indicating whether this instance is initializer event.
    /// </summary>
    /// <value><c>true</c> if this instance is initializer event; otherwise, <c>false</c>.</value>
    public bool IsInitializerEvent { get; }

    [Obsolete]
    private static ApplicationError GetError(IDomainAggregate? aggregate, BaseEvent domainEvent, bool isInitializerEvent, string? message)
        => isInitializerEvent
            ? new ApplicationError
            {
                Title = "Error applying event",
                Detail = "The Event '{MessageType}' cannot be applied to the aggregate '{AggregateName}' with id '{AggregateId}'."
                    + (
                string.IsNullOrWhiteSpace(message)
                        ? string.Empty
                        : "\n" + message),
                Arguments = [domainEvent.TypeName, aggregate?.AggregateName ?? domainEvent.AggregateName, aggregate?.AggregateId ?? domainEvent.AggregateId],
                Category = ErrorCategory.Functional,
            }
            : new ApplicationError
            {
                Title = "Error applying event",
                Detail = "The event '{MessageType}' can only be used for initialization. The aggregate '{AggregateName}' with ID '{aggregateId}' is already initialized.",
                Arguments = [domainEvent.TypeName, domainEvent.AggregateName, domainEvent.AggregateId],
                Category = ErrorCategory.Functional,
            };
}