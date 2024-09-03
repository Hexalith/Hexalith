// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-21-2023
// ***********************************************************************
// <copyright file="SurveyEvent.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
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
/// Class SurveyEvent.
/// Implements the <see cref="Hexalith.Domain.Events.CompanyEntityEvent" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Events.CompanyEntityEvent" />
[DataContract]
public abstract class SurveyEvent : CompanyEntityEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SurveyEvent"/> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    [JsonConstructor]
    protected SurveyEvent(string partitionId, string companyId, string originId, string id)
        : base(partitionId, companyId, originId, id)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SurveyEvent" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected SurveyEvent()
    {
    }

    /// <inheritdoc/>
    protected override string DefaultAggregateId()
        => Survey.GetAggregateId(PartitionId, CompanyId, OriginId, Id);

    /// <inheritdoc/>
    protected override string DefaultAggregateName()
        => Survey.GetAggregateName();
}