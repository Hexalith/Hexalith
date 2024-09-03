// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis
// Author           : Jérôme Piquot
// Created          : 11-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-04-2023
// ***********************************************************************
// <copyright file="BusTopicAttribute.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

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
[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public abstract class BusTopicAttribute : TopicAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BusTopicAttribute" /> class.
    /// </summary>
    /// <param name="pubsubName">The name of the pubsub.</param>
    /// <param name="name">The name of the topic.</param>
    protected BusTopicAttribute(string pubsubName, string name)
        : base(
            (pubsubName ?? throw new ArgumentNullException(nameof(pubsubName))).ToLowerInvariant(),
            (name ?? throw new ArgumentNullException(nameof(name))).ToLowerInvariant())
    {
    }
}