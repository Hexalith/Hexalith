// <copyright file="CustomerNotRegisteredError.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Parties.Errors;

using System.Runtime.Serialization;

using Hexalith.Extensions.Common;

/// <summary>
/// Represents an error that occurs when trying to apply an event on a non-registered customer.
/// </summary>
/// <remarks>
/// This error is thrown when an event is attempted to be applied on a customer that does not exist.
/// </remarks>
/// <seealso cref="Hexalith.ApplicationError" />
[DataContract]
public record CustomerNotRegisteredError : ApplicationError
{
    /// <summary>
    /// Gets the ID of the aggregate.
    /// </summary>
    [DataMember(Order = 2)]
    public string? AggregateId { get; init; }

    /// <summary>
    /// Gets the name of the event type.
    /// </summary>
    [DataMember(Order = 1)]
    public string? EventTypeName { get; init; }

    /// <summary>
    /// Creates a new instance of the <see cref="CustomerNotRegisteredError"/> class.
    /// </summary>
    /// <param name="eventTypeName">The name of the event type.</param>
    /// <param name="aggregateId">The ID of the aggregate.</param>
    /// <returns>A new instance of the <see cref="CustomerNotRegisteredError"/> class.</returns>
    public static CustomerNotRegisteredError Create(string eventTypeName, string aggregateId)
    {
        return new CustomerNotRegisteredError
        {
            AggregateId = aggregateId,
            Title = "Customer not registered",
            Detail = "The event {EventTypeName} with id '{AggregateId}' can only be applied on an existing customer.",
            Category = ErrorCategory.Functional,
            Arguments = [eventTypeName, aggregateId],
            EventTypeName = eventTypeName,
        };
    }
}