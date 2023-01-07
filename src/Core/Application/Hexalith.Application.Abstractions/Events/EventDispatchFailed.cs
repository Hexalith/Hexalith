// <copyright file="EventDispatchFailed.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Events;

using Ardalis.GuardClauses;

using Hexalith.Application.Abstractions.Errors;
using Hexalith.Domain.Abstractions.Events;
using Hexalith.Extensions.Helpers;

using System;
using System.Runtime.Serialization;

[DataContract]
public record EventDispatchFailed : Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventDispatchFailed"/> class.
    /// </summary>
    /// <param name="event"></param>
    /// <param name="ex"></param>
    public EventDispatchFailed(IEvent @event, Exception ex)
    {
        _ = Guard.Against.Null(ex);
        Title = "Event dispatch failed";
        Type = nameof(EventDispatchFailed);
        Detail = "Could not dispatch event {EventName} with aggregate id {AggregateName}-{EventId}.";
        Arguments = new object[] { @event.MessageName, @event.AggregateName, @event.AggregateId };
        TechnicalDetail = "Could not dispatch event {EventName} with aggregate id {AggregateName}-{EventId}:\n{ErrorMessage}\n{StackTrace}";
        TechnicalArguments = new object[]
        {
            @event.MessageName,
            @event.AggregateName,
            @event.AggregateId,
            ex.FullMessage(),
            ex.StackTrace ?? string.Empty,
        };
    }
}
