// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-29-2023
// ***********************************************************************
// <copyright file="SurveyRegistered.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Events;

using System.Runtime.Serialization;

using Hexalith.Domain.Entities;
using Hexalith.Domain.ValueObjects;
using Hexalith.Extensions;

/// <summary>
/// Class SurveyRegistered.
/// Implements the <see cref="Hexalith.Domain.Events.SurveyEvent" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Events.SurveyEvent" />
[DataContract]
public class SurveyRegistered : SurveyEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SurveyRegistered" /> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="name">The name.</param>
    /// <param name="categories">The categories.</param>
    /// <param name="users">The users.</param>
    /// <param name="period">The period.</param>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    public SurveyRegistered(
        string partitionId,
        string companyId,
        string originId,
        string id,
        string name,
        IEnumerable<SurveyCategory> categories,
        IEnumerable<SurveyUser> users,
        SurveyPeriod period,
        DateTimeOffset startDate,
        DateTimeOffset endDate)
        : base(partitionId, companyId, originId, id)
    {
        Name = name;
        Categories = categories;
        Users = users;
        Period = period;
        StartDate = startDate;
        EndDate = endDate;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SurveyRegistered" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public SurveyRegistered()
    {
        Name = string.Empty;
        StartDate = EndDate = DateTimeOffset.MinValue;
        Period = SurveyPeriod.Once;
        Categories = [];
        Users = [];
    }

    /// <summary>
    /// Gets the categories.
    /// </summary>
    /// <value>The categories.</value>
    [DataMember(Order = 15)]
    public IEnumerable<SurveyCategory> Categories { get; }

    /// <summary>
    /// Gets or sets the end date.
    /// </summary>
    /// <value>The end date.</value>
    [DataMember(Order = 13)]
    public DateTimeOffset EndDate { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [DataMember(Order = 10)]
    public string Name { get; set; }

    /// <summary>
    /// Gets the period.
    /// </summary>
    /// <value>The period.</value>
    [DataMember(Order = 11)]
    public SurveyPeriod Period { get; }

    /// <summary>
    /// Gets or sets the external ids.
    /// </summary>
    /// <value>The external ids.</value>
    [DataMember(Order = 12)]
    public DateTimeOffset StartDate { get; set; }

    /// <summary>
    /// Gets the users.
    /// </summary>
    /// <value>The users.</value>
    [DataMember(Order = 14)]
    public IEnumerable<SurveyUser> Users { get; }
}