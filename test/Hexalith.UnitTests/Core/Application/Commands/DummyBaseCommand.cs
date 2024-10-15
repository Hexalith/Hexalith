// <copyright file="DummyBaseCommand.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Commands;

using Hexalith.Application.MessageMetadatas;
using Hexalith.Extensions.Helpers;
using Hexalith.PolymorphicSerialization;

[PolymorphicSerialization]
public partial record DummyBaseCommand(string BaseValue)
{
    public Metadata CreateMetadata()
    {
        return new Metadata(
            new MessageMetadata(
                UniqueIdHelper.GenerateUniqueStringId(),
                nameof(DummyBaseCommand),
                3,
                new AggregateMetadata(AggregateName, AggregateId),
                DateTimeOffset.UtcNow),
            new ContextMetadata(
                UniqueIdHelper.GenerateUniqueStringId(),
                "Test user",
                "PART1",
                DateTimeOffset.UtcNow.AddMinutes(-1),
                100,
                "SESS222",
                ["TestScope"]));
    }

    public string AggregateName => "Test";

    public string AggregateId => BaseValue;
}