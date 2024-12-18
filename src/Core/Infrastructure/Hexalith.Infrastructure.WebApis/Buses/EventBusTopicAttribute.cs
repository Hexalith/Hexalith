// <copyright file="EventBusTopicAttribute.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis.Buses;

using Dapr;

using Hexalith.Application;

#pragma warning disable S1694 // An abstract class should have both abstract and concrete methods
#pragma warning disable CA1308 // Normalize strings to uppercase

/// <summary>
/// Class BusTopicAttribute. This class cannot be inherited.
/// Implements the <see cref="TopicAttribute" />.
/// </summary>
/// <seealso cref="TopicAttribute" />
[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public class EventBusTopicAttribute : BusTopicAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventBusTopicAttribute" /> class.
    /// </summary>
    /// <param name="aggregateName">Name of the aggregate.</param>
    public EventBusTopicAttribute(string aggregateName)
        : base(ApplicationConstants.EventBus, aggregateName + ApplicationConstants.EventBusSuffix)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(aggregateName);
        AggregateName = aggregateName.ToLowerInvariant();
    }

    /// <summary>
    /// Gets the name of the aggregate.
    /// </summary>
    /// <value>The name of the aggregate.</value>
    public string AggregateName { get; }
}