﻿// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis
// Author           : Jérôme Piquot
// Created          : 11-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-04-2023
// ***********************************************************************
// <copyright file="BusTopicAttribute.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.Buses;

using Dapr;

/// <summary>
/// Class BusTopicAttribute. This class cannot be inherited.
/// Implements the <see cref="TopicAttribute" />.
/// </summary>
/// <seealso cref="TopicAttribute" />
#pragma warning disable CA1308 // Normalize strings to uppercase

[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public abstract class BusTopicAttribute : TopicAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BusTopicAttribute" /> class.
    /// </summary>
    /// <param name="pubsubName">Name of the pubsub.</param>
    /// <param name="name">The name.</param>
    protected BusTopicAttribute(string pubsubName, string name)
        : base(
            (pubsubName ?? throw new ArgumentNullException(nameof(pubsubName))).ToLowerInvariant(),
            (name ?? throw new ArgumentNullException(nameof(name))).ToLowerInvariant())
    {
    }
}