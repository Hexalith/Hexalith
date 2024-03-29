﻿// <copyright file="Role.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Users.Aggregates;

using System;
using System.Runtime.Serialization;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Domain.Users.Models;

/// <summary>
/// Represents a role.
/// Implements the <see cref="Aggregate" />
/// Implements the <see cref="IAggregate" />
/// Implements the <see cref="System.IEquatable{Hexalith.Domain.Aggregates.Aggregate}" />
/// Implements the <see cref="System.IEquatable{Hexalith.Domain.Users.Aggregates.Role}" />.
/// </summary>
/// <seealso cref="Aggregate" />
/// <seealso cref="IAggregate" />
/// <seealso cref="System.IEquatable{Hexalith.Domain.Aggregates.Aggregate}" />
/// <seealso cref="System.IEquatable{Hexalith.Domain.Users.Aggregates.Role}" />
[DataContract]
public record Role([property: DataMember(Order = 1)] string Id, [property: DataMember(Order = 2)] string Name, [property: DataMember(Order = 3)] string Description) : Aggregate, IRole
{
    /// <inheritdoc/>
    public override (IAggregate Aggregate, IEnumerable<BaseEvent> Events) Apply(BaseEvent domainEvent) => throw new NotImplementedException();

    /// <inheritdoc/>
    public override bool IsInitialized() => throw new NotImplementedException();
}