// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-28-2023
// ***********************************************************************
// <copyright file="ExternalSystemReferenceAdded.cs" company="Fiveforty SAS Paris France">
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
/// Class ExternalSystemReferenceMapped.
/// Implements the <see cref="Hexalith.Domain.Events.ExternalSystemEvent" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Events.ExternalSystemEvent" />
[DataContract]
[Serializable]
public class ExternalSystemReferenceAdded : ExternalSystemEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalSystemReferenceAdded"/> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="systemId">The system identifier.</param>
    /// <param name="referenceAggregateName">Name of the reference aggregate.</param>
    /// <param name="externalId">The external identifier.</param>
    /// <param name="referenceAggregateId">The reference aggregate identifier.</param>
    [JsonConstructor]
    public ExternalSystemReferenceAdded(
        string partitionId,
        string companyId,
        string systemId,
        string referenceAggregateName,
        string externalId,
        string referenceAggregateId)
        : base(partitionId, companyId, systemId, referenceAggregateName, externalId, referenceAggregateId) => ReferenceAggregateId = referenceAggregateId;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalSystemReferenceAdded" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public ExternalSystemReferenceAdded()
    {
    }
}