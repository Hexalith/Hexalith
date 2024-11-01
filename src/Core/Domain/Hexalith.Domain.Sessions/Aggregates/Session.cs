// <copyright file="Session.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Sessions.Aggregates;

using Hexalith.Domain.Aggregates;

public record Session(
    string Id,
    string UserId,
    string PartitionId,
    DateTimeOffset CreatedOn,
    DateTimeOffset LastAccessedOn,
    TimeSpan Timeout,
    TimeSpan Lifetime)
    : IDomainAggregate
{
    /// <inheritdoc/>
    public string AggregateId => Id;

    /// <inheritdoc/>
    public string? AggregateName => SessionsConstants.SessionAggregateName;

    /// <inheritdoc/>
    public ApplyResult Apply(object domainEvent) => throw new NotImplementedException();

    /// <inheritdoc/>
    public bool IsInitialized() => throw new NotImplementedException();
}