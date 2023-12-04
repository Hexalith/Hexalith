// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-21-2023
// ***********************************************************************
// <copyright file="ExternalCustomerMapped.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Events;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Class ExternalCustomerMapped.
/// Implements the <see cref="Hexalith.Domain.Events.ExternalCustomerEvent" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Events.ExternalCustomerEvent" />
[DataContract]
public class ExternalCustomerMapped : ExternalCustomerEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalCustomerMapped" /> class.
    /// </summary>
    /// <param name="system">The system.</param>
    /// <param name="externalId">The external identifier.</param>
    /// <param name="id">The identifier.</param>
    [JsonConstructor]
    public ExternalCustomerMapped(string system, string externalId, string id)
        : base(system, externalId) => Id = id;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalCustomerMapped" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public ExternalCustomerMapped() => Id = string.Empty;

    /// <summary>
    /// Gets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember(Order = 10)]
    [JsonPropertyOrder(10)]
    public string Id { get; private set; }
}