// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.ExternalSystemsEvents
// Author           : Jérôme Piquot
// Created          : 11-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-04-2023
// ***********************************************************************
// <copyright file="ExternalSystemReferenceEventsBusTopicAttribute.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.ExternalSystemsEvents.Controllers;

using Hexalith.Domain.Aggregates;
using Hexalith.Infrastructure.WebApis.Buses;

/// <summary>
/// Class ExternalSystemReferenceEventsBusTopicAttribute. This class cannot be inherited.
/// Implements the <see cref="EventBusTopicAttribute" />.
/// </summary>
/// <seealso cref="EventBusTopicAttribute" />
[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public sealed class ExternalSystemReferenceEventsBusTopicAttribute : EventBusTopicAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalSystemReferenceEventsBusTopicAttribute"/> class.
    /// </summary>
    public ExternalSystemReferenceEventsBusTopicAttribute()
        : base(ExternalSystemReference.GetAggregateName())
    {
    }
}