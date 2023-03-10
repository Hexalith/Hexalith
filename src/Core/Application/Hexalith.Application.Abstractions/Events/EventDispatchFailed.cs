// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-06-2023
// ***********************************************************************
// <copyright file="EventDispatchFailed.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Abstractions.Events;

using System;
using System.Runtime.Serialization;

using Ardalis.GuardClauses;

using Hexalith.Domain.Abstractions.Events;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;

/// <summary>
/// Class EventDispatchFailed.
/// Implements the <see cref="Error" />
/// Implements the <see cref="System.IEquatable{Hexalith.Extensions.Common.Error}" />
/// Implements the <see cref="System.IEquatable{Hexalith.Application.Abstractions.Events.EventDispatchFailed}" />.
/// </summary>
/// <seealso cref="Error" />
/// <seealso cref="System.IEquatable{Hexalith.Extensions.Common.Error}" />
/// <seealso cref="System.IEquatable{Hexalith.Application.Abstractions.Events.EventDispatchFailed}" />
[DataContract]
public record EventDispatchFailed : Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventDispatchFailed" /> class.
    /// </summary>
    /// <param name="event">The event.</param>
    /// <param name="ex">The ex.</param>
    public EventDispatchFailed(IEvent @event, Exception ex)
    {
        _ = Guard.Against.Null(ex);
        Title = "Event dispatch failed";
        Type = nameof(EventDispatchFailed);
        Detail = "Could not dispatch event {EventName} with aggregate id {AggregateName}-{EventId}.";
        Arguments = new object[] { @event.TypeName, @event.AggregateName, @event.AggregateId };
        TechnicalDetail = "Could not dispatch event {EventName} with aggregate id {AggregateName}-{EventId}:\n{ErrorMessage}\n{StackTrace}";
        TechnicalArguments = new object[]
        {
            @event.TypeName,
            @event.AggregateName,
            @event.AggregateId,
            ex.FullMessage(),
            ex.StackTrace ?? string.Empty,
        };
    }
}