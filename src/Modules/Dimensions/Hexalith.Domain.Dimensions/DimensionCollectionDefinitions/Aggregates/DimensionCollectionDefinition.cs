﻿// <copyright file="DimensionCollectionDefinition.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Dimensions.DimensionCollectionDefinitions.Aggregates;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Dimensions.DimensionCollectionDefinitions.Entities;
using Hexalith.Domain.Dimensions.DimensionCollectionDefinitions.Events;
using Hexalith.Domain.Events;

/// <summary>
/// Class DimensionDefinition.
/// Implements the <see cref="Aggregate" />
/// Implements the <see cref="IDomainAggregate" />.
/// </summary>
/// <seealso cref="Aggregate" />
/// <seealso cref="IDomainAggregate" />
[DataContract]
public record DimensionCollectionDefinition(
    string Id,
    [property: DataMember(Order = 3)] string Name,
    [property: DataMember(Order = 4)] string? Description,
    [property: DataMember(Order = 5)] IEnumerable<DimensionDefinition> Values) : IDomainAggregate
{
    /// <inheritdoc/>
    public bool IsInitialized() => !string.IsNullOrWhiteSpace(Id);

    /// <inheritdoc/>
    public string AggregateId => DimensionDomainHelper.BuildDimensionCollectionDefinitionAggregateId(Id);

    /// <inheritdoc/>
    public string AggregateName => DimensionDomainHelper.DimensionCollectionDefinitionAggregateName;

    /// <inheritdoc/>
    public DimensionCollectionDefinition(DimensionCollectionDefinitionAdded added)
        : this((added ?? throw new ArgumentNullException(nameof(added))).Id, added.Name, added.Description, [])
    {
    }

    /// <inheritdoc/>
    public ApplyResult Apply([NotNull] object domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        if (domainEvent is DimensionCollectionDefinitionAdded added)
        {
            return !IsInitialized()
                ? ApplyEvent(added)
                : new ApplyResult(
                this,
                [new DimensionCollectionDefinitionEventCancelled(added, $"Aggregate {Id}/{Name} already initialized")],
                true);
        }

        if (domainEvent is DimensionCollectionDefinitionEvent contactEvent)
        {
            if (contactEvent.AggregateId != AggregateId)
            {
                return new ApplyResult(this, [new DimensionCollectionDefinitionEventCancelled(contactEvent, $"Invalid aggregate identifier for {Id}/{Name} : {contactEvent.AggregateId}")], true);
            }
        }
        else
        {
            return new ApplyResult(
                this,
                [new InvalidEventApplied(
                    AggregateName,
                    AggregateId,
                    domainEvent.GetType().FullName ?? "Unknown",
                    JsonSerializer.Serialize(domainEvent),
                    $"Unexpected event applied.")],
                true);
        }

        return contactEvent switch
        {
            DimensionCollectionDefinitionInformationChanged e => ApplyEvent(e),
            _ => new ApplyResult(
                this,
                [new DimensionCollectionDefinitionEventCancelled(contactEvent, "Event not implemented")],
                true),
        };
    }

    private ApplyResult ApplyEvent(DimensionCollectionDefinitionAdded added)
        => new(new DimensionCollectionDefinition(added), [added], false);

    private ApplyResult ApplyEvent(DimensionCollectionDefinitionInformationChanged changed)
        => new(this with { Name = changed.Name, Description = changed.Description }, [changed], false);
}