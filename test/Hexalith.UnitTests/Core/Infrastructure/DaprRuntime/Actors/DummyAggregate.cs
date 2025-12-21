// <copyright file="DummyAggregate.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.DaprRuntime.Actors;

using System.Runtime.Serialization;

using Hexalith.Domains;
using Hexalith.Domains.Results;

[DataContract]
public record DummyAggregate(string Id, string Name) : IDomainAggregate
{
    public DummyAggregate()
        : this(string.Empty, string.Empty)
    {
    }

    public string DomainName => GetAggregateName();

    public string DomainId => GetAggregateId(Id);

    public bool IsInitialized() => !string.IsNullOrWhiteSpace(Id);

    public static string GetAggregateId(string id) => id;

    public static string GetAggregateName() => "Dummy";

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