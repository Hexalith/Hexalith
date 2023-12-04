// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-21-2023
// ***********************************************************************
// <copyright file="ExternalCustomerEvent.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Events;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.Aggregates;
using Hexalith.Extensions;

/// <summary>
/// Class ExternalCustomerEvent.
/// Implements the <see cref="Hexalith.Domain.Events.BaseEvent" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Events.BaseEvent" />
[DataContract]
public abstract class ExternalCustomerEvent : BaseEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalCustomerEvent" /> class.
    /// </summary>
    /// <param name="system">The system.</param>
    /// <param name="externalId">The external identifier.</param>
    protected ExternalCustomerEvent(string system, string externalId)
    {
        System = system;
        ExternalId = externalId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalCustomerEvent" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected ExternalCustomerEvent()
    {
        ExternalId = string.Empty;
        System = string.Empty;
    }

    /// <summary>
    /// Gets the external identifier.
    /// </summary>
    /// <value>The external identifier.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public string ExternalId { get; }

    /// <summary>
    /// Gets the system.
    /// </summary>
    /// <value>The system.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string System { get; }

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => base.DefaultAggregateName() + Separator + System + Separator + ExternalId;

    /// <inheritdoc/>
    protected override string DefaultAggregateName() => nameof(ExternalCustomer);
}