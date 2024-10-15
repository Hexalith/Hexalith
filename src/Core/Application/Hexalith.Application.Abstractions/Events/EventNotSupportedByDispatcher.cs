// <copyright file="EventNotSupportedByDispatcher.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Events;

using System.Runtime.Serialization;

using Hexalith.Extensions.Common;

/// <summary>
/// Class EventNotSupportedByDispatcher.
/// Implements the <see cref="ApplicationError" />
/// Implements the <see cref="IEquatable{Error}" />
/// Implements the <see cref="IEquatable{EventNotSupportedByDispatcher}" />.
/// </summary>
/// <seealso cref="ApplicationError" />
/// <seealso cref="IEquatable{Error}" />
/// <seealso cref="IEquatable{EventNotSupportedByDispatcher}" />
[DataContract]
public record EventNotSupportedByDispatcher : ApplicationError
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventNotSupportedByDispatcher" /> class.
    /// </summary>
    /// <param name="dispatcher">The dispatcher.</param>
    public EventNotSupportedByDispatcher(string dispatcher)
    {
        Title = "Dispatcher event unsupported.";
        Type = nameof(EventNotSupportedByDispatcher);
        Detail = "The event could not be handled. It has an unsupported format.";
        TechnicalDetail = "The event is unsupported by {DispatcherName}.";
        TechnicalArguments = [dispatcher];
    }
}