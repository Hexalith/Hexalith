﻿// <copyright file="DummyAggregateEvent1.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.DaprRuntime.Actors;

using System.Runtime.Serialization;

using Hexalith.Domain.Events;

[DataContract]
[Serializable]
public class DummyAggregateEvent1 : BaseEvent
{
    public string Id { get; set; }

    public string Name { get; set; } = string.Empty;

    protected override string DefaultAggregateId() => DummyAggregate.GetAggregateId(Id);

    protected override string DefaultAggregateName() => DummyAggregate.GetAggregateName();
}