// <copyright file="DummyAggregateCommand1.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.DaprRuntime.Actors;

using Hexalith.PolymorphicSerializations;

[PolymorphicSerialization]
public partial record DummyAggregateCommand1(string Id, string Name)
{
    public string DomainId => DummyAggregate.GetAggregateId(Id);

    public string DomainName => DummyAggregate.GetAggregateName();
}