﻿// <copyright file="DummyBaseMessage.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Domain.Messages;

using Hexalith.Application.Metadatas;
using Hexalith.Extensions.Helpers;
using Hexalith.PolymorphicSerializations;

[PolymorphicSerialization]
public abstract partial record DummyBaseMessage(string BaseValue)
{
    public string AggregateName => "Test";

    public virtual string AggregateId => BaseValue;

    public Metadata CreateMetadata()
    {
        return new Metadata(
                MessageMetadata.Create(this, DateTimeOffset.UtcNow),
                new ContextMetadata(
                    UniqueIdHelper.GenerateUniqueStringId(),
                    "Test user",
                    "PART1",
                    DateTimeOffset.UtcNow.AddMinutes(-1),
                    1,
                    "Test session",
                    ["TestScope"]));
    }
}