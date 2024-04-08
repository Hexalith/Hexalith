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
    public DummyAggregate()
        : this(string.Empty)
    {
    }

    public override (IAggregate Aggregate, IEnumerable<BaseEvent> Events) Apply(BaseEvent domainEvent)
    {
        return domainEvent is DummyAggregateEvent1 dummyEvent
            ? (this with
            {
                Id = dummyEvent.Id,
            }, [domainEvent])
            : throw new NotImplementedException();
    }

    public override bool IsInitialized() => !string.IsNullOrWhiteSpace(Id);

    public static string GetAggregateId(string id) => GetAggregateName() + Separator + id;

#pragma warning disable CA1024 // Use properties where appropriate
    public static string GetAggregateName() => "Dummy";
#pragma warning restore CA1024 // Use properties where appropriate

    protected override string DefaultAggregateName() => GetAggregateName();

    protected override string DefaultAggregateId() => GetAggregateId(Id);
}