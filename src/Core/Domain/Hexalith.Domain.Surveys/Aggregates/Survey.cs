﻿// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-29-2023
// ***********************************************************************
// <copyright file="Survey.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Aggregates;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

using Hexalith.Domain.Entities;
using Hexalith.Domain.Events;
using Hexalith.Domain.Exceptions;
using Hexalith.Domain.Messages;
using Hexalith.Domain.ValueObjects;
using Hexalith.Extensions;

/// <summary>
/// Class Survey.
/// Implements the <see cref="Hexalith.Domain.Aggregates.Aggregate" />
/// Implements the <see cref="Hexalith.Domain.Aggregates.IAggregate" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Aggregates.Aggregate" />
/// <seealso cref="Hexalith.Domain.Aggregates.IAggregate" />
[DataContract]
public record Survey(
    string PartitionId,
    string CompanyId,
    string OriginId,
    string Id,
    [property: DataMember(Order = 5)] string Name,
    [property: DataMember(Order = 6)] IEnumerable<SurveyCategory> Categories,
    [property: DataMember(Order = 7)] IEnumerable<SurveyUser> Users,
    [property: DataMember(Order = 8)] SurveyPeriod Period,
    [property: DataMember(Order = 9)] DateTimeOffset StartDate,
    [property: DataMember(Order = 10)] DateTimeOffset EndDate) : EntityAggregate(PartitionId, CompanyId, OriginId, Id)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Survey"/> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public Survey()
        : this(
              string.Empty,
              string.Empty,
              string.Empty,
              string.Empty,
              string.Empty,
              [],
              [],
              SurveyPeriod.Once,
              DateTimeOffset.MinValue,
              DateTimeOffset.MinValue)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Survey" /> class.
    /// </summary>
    /// <param name="customer">The customer.</param>
    public Survey(SurveyRegistered customer)
        : this(
              (customer ?? throw new ArgumentNullException(nameof(customer))).PartitionId,
              customer.CompanyId,
              customer.OriginId,
              customer.Id,
              customer.Name,
              customer.Categories,
              customer.Users,
              customer.Period,
              customer.StartDate,
              customer.EndDate)
    {
    }

    /// <inheritdoc/>
    public override (IAggregate Aggregate, IEnumerable<BaseMessage> Messages) Apply(BaseEvent domainEvent)
    {
        return (domainEvent switch
        {
            SurveyInformationChanged changed => this with
            {
                Name = changed.Name,
            },
            SurveyRegistered => throw new InvalidAggregateEventException(this, domainEvent, true),
            _ => throw new InvalidAggregateEventException(this, domainEvent, false),
        }, []);
    }

    /// <summary>
    /// Gets the aggregate identifier.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <returns>System.String.</returns>
    public static string GetAggregateId([NotNull] string partitionId, [NotNull] string companyId, [NotNull] string originId, [NotNull] string id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(partitionId);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyId);
        ArgumentException.ThrowIfNullOrWhiteSpace(originId);
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        return Normalize(GetAggregateName() + Separator + partitionId + Separator + companyId + Separator + originId + Separator + id);
    }

    /// <summary>
    /// Gets the name of the aggregate.
    /// </summary>
    /// <returns>System.String.</returns>
#pragma warning disable CA1024 // Use properties where appropriate
    public static string GetAggregateName() => nameof(Survey);
#pragma warning restore CA1024 // Use properties where appropriate

    /// <summary>
    /// Determines whether the specified changed has changes.
    /// </summary>
    /// <param name="changed">The changed.</param>
    /// <returns><c>true</c> if the specified changed has changes; otherwise, <c>false</c>.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public bool HasChanges([NotNull] SurveyInformationChanged changed)
    {
        ArgumentNullException.ThrowIfNull(changed);
        CheckEvent(changed);
        return Name != changed.Name;
    }

    /// <inheritdoc/>
    public override bool IsInitialized() => !string.IsNullOrWhiteSpace(Id);

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => GetAggregateId(PartitionId, CompanyId, OriginId, Id);

    private void CheckEvent(SurveyEvent customerEvent, [CallerArgumentExpression(nameof(customerEvent))] string? paramName = null)
    {
        if (AggregateName != customerEvent.AggregateName)
        {
            throw new ArgumentException($"{customerEvent.TypeName} can not be applied to aggregate {AggregateName}.", paramName);
        }

        if (AggregateId != customerEvent.AggregateId)
        {
            throw new ArgumentException($"{customerEvent.TypeName} aggregate aggregate Id '{customerEvent.AggregateId}' is invalid. Expected : '{AggregateId}'.", paramName);
        }
    }
}