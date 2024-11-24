// <copyright file="BusTopicAttribute.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis.Buses;

using Dapr;

#pragma warning disable S1694 // An abstract class should have both abstract and concrete methods
#pragma warning disable CA1308 // Normalize strings to uppercase

/// <summary>
/// Class BusTopicAttribute. This class cannot be inherited.
/// Implements the <see cref="TopicAttribute" />.
/// </summary>
/// <seealso cref="TopicAttribute" />
/// <remarks>
/// Initializes a new instance of the <see cref="BusTopicAttribute" /> class.
/// </remarks>
/// <param name="pubsubName">The name of the pubsub.</param>
/// <param name="name">The name of the topic.</param>
[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public abstract class BusTopicAttribute(string pubsubName, string name) : TopicAttribute(
        (pubsubName ?? throw new ArgumentNullException(nameof(pubsubName))).ToLowerInvariant(),
        (name ?? throw new ArgumentNullException(nameof(name))).ToLowerInvariant())
{
}