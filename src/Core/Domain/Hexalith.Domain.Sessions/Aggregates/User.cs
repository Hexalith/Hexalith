// <copyright file="User.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Sessions.Aggregates;

using System;
using System.Runtime.Serialization;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Sessions;

[DataContract]
public record User(string Id, string Name, string ContactId, IEnumerable<string> Roles) : IDomainAggregate
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