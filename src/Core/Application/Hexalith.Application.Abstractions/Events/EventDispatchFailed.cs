// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-06-2023
// ***********************************************************************
// <copyright file="EventDispatchFailed.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Events;

using System;
using System.Runtime.Serialization;

using Hexalith.Domain.Events;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;

/// <summary>
/// Class EventDispatchFailed.
/// Implements the <see cref="ApplicationError" />
/// Implements the <see cref="IEquatable{Error}" />
/// Implements the <see cref="IEquatable{EventDispatchFailed}" />.
/// </summary>
/// <seealso cref="ApplicationError" />
/// <seealso cref="IEquatable{Error}" />
/// <seealso cref="IEquatable{EventDispatchFailed}" />
[DataContract]
public record EventDispatchFailed : ApplicationError
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventDispatchFailed" /> class.
    /// </summary>
    /// <param name="event">The event.</param>
    /// <param name="ex">The ex.</param>
    public EventDispatchFailed(IEvent @event, Exception ex)
    {
        ArgumentNullException.ThrowIfNull(ex);
        ArgumentNullException.ThrowIfNull(@event);
        Title = "Event dispatch failed";
        Type = nameof(EventDispatchFailed);
        Detail = "Could not dispatch event {EventName} with aggregate id {EventId}.";
        Arguments = [@event.TypeName, @event.AggregateId];
        TechnicalDetail = "Could not dispatch event {EventName} with aggregate id {EventId}:\n{ErrorMessage}\n{StackTrace}";
        TechnicalArguments = [
            @event.TypeName,
            @event.AggregateId,
            ex.FullMessage(),
            ex.StackTrace ?? string.Empty,
        ];
    }
}