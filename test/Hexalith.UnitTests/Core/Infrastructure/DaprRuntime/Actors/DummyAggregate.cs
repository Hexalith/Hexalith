// <copyright file="DummyAggregate.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.DaprRuntime.Actors;

using System.Runtime.Serialization;

using Hexalith.Domain.Aggregates;

[DataContract]
public record DummyAggregate(string Id, string Name) : IDomainAggregate
{
    public DummyAggregate()
        : this(string.Empty, string.Empty)
    {
    }

    public bool IsInitialized() => !string.IsNullOrWhiteSpace(Id);

    public static string GetAggregateId(string id) => GetAggregateName() + "-" + id;

    public static string GetAggregateName() => "Dummy";

    public string AggregateName => GetAggregateName();

    public string AggregateId => GetAggregateId(Id);

    ApplyResult IDomainAggregate.Apply(object domainEvent)
        => domainEvent is DummyAggregateEvent1 dummyEvent
            ? new ApplyResult(
                this with
                {
                    Id = dummyEvent.Id,
                    Name = dummyEvent.Name,
                },
                [domainEvent],
                false)
            : throw new NotImplementedException();
}