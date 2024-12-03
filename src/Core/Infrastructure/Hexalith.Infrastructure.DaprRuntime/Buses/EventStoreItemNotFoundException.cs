// <copyright file="EventStoreItemNotFoundException.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Buses;

using System;
using System.Runtime.Serialization;

using Hexalith.Extensions.Helpers;

/// <summary>
/// The event store item not found exception.
/// </summary>
[DataContract]
public class EventStoreItemNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventStoreItemNotFoundException"/> class.
    /// </summary>
    public EventStoreItemNotFoundException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventStoreItemNotFoundException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public EventStoreItemNotFoundException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventStoreItemNotFoundException"/> class.
    /// </summary>
    /// <param name="version">The item version.</param>
    /// <param name="streamName">The stream name.</param>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public EventStoreItemNotFoundException(long version, string streamName, string? message, Exception? innerException)
        : base($"The item with version {version.ToInvariantString()} not found in event store '{streamName}'. " + message, innerException)
    {
        Version = version;
        StreamName = streamName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventStoreItemNotFoundException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public EventStoreItemNotFoundException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Gets the stream name.
    /// </summary>
    public string? StreamName { get; private set; }

    /// <summary>
    /// Gets the stream item version.
    /// </summary>
    public long? Version { get; private set; }
}