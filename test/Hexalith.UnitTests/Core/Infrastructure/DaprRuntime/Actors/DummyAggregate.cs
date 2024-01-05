// <copyright file="DummyAggregate.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.DaprRuntime.Actors;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;

public record DummyAggregate(string Id) : Aggregate
{
    protected override string DefaultAggregateId() => GetAggregateId(Id);

    public static string GetAggregateId(string id) => GetAggregateName() + Separator + id;

    public static string GetAggregateName() => "Dummy";

    protected override string DefaultAggregateName() => GetAggregateName();

    public override IAggregate Apply(BaseEvent domainEvent)
    {
        return domainEvent is DummyAggregateEvent1 dummyEvent
            ? (IAggregate)(this with
            {
                Id = dummyEvent.Id,
            })
            : throw new NotImplementedException();
    }
}