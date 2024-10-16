// <copyright file="DummyBaseEvent.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Domain.Events;

using Hexalith.Application.MessageMetadatas;
using Hexalith.Extensions.Helpers;
using Hexalith.PolymorphicSerialization;

[PolymorphicSerialization]
public abstract partial record DummyBaseEvent(string BaseValue)
{
    public Metadata CreateMetadata()
    {
        return new Metadata(
                new MessageMetadata(this, DateTimeOffset.UtcNow),
                new ContextMetadata(
                    UniqueIdHelper.GenerateUniqueStringId(),
                    "Test user",
                    "PART1",
                    DateTimeOffset.UtcNow.AddMinutes(-1),
                    1,
                    "Test session",
                    ["TestScope"]));
    }

    public string AggregateName => "Test";

    public virtual string AggregateId => BaseValue;
}