// ***********************************************************************
// Assembly         : Hexalith.Domain.Abstractions
// Author           : Jérôme Piquot
// Created          : 04-28-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-28-2023
// ***********************************************************************
// <copyright file="InvalidAggregateEventException.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Abstractions.Exceptions;

using System;
using System.Runtime.Serialization;

using Hexalith.Domain.Abstractions.Aggregates;
using Hexalith.Domain.Abstractions.Events;

/// <summary>
/// Class InvalidAggregateEventException.
/// Implements the <see cref="InvalidOperationException" />.
/// </summary>
/// <seealso cref="InvalidOperationException" />
public class InvalidAggregateEventException : InvalidOperationException
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
    public InvalidAggregateEventException(IAggregate aggregate, BaseEvent domainEvent, bool isInitializerEvent)
        : base($"Event {domainEvent?.TypeName} is not supported by aggregate {aggregate?.AggregateName}." +
            (isInitializerEvent ? $" This event can only be used once, to initialize the aggregate." : string.Empty))
    {
        Aggregate = aggregate;
        DomainEvent = domainEvent;
        IsInitializerEvent = isInitializerEvent;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidAggregateEventException"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not a null reference (<see langword="Nothing" /> in Visual Basic), the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
    public InvalidAggregateEventException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidAggregateEventException"/> class.
    /// </summary>
    /// <param name="info">The object that holds the serialized object data.</param>
    /// <param name="context">The contextual information about the source or destination.</param>
    protected InvalidAggregateEventException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    /// <summary>
    /// Gets the aggregate.
    /// </summary>
    /// <value>The aggregate.</value>
    public IAggregate? Aggregate { get; }

    /// <summary>
    /// Gets the domain event.
    /// </summary>
    /// <value>The domain event.</value>
    public BaseEvent? DomainEvent { get; }

    public bool IsInitializerEvent { get; }
}