// <copyright file="DummyBaseRequest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Requests;

using System.Runtime.Serialization;

using Hexalith.Commons.Metadatas;
using Hexalith.Commons.UniqueIds;
using Hexalith.Extensions.Helpers;
using Hexalith.PolymorphicSerializations;

[PolymorphicSerialization]
public abstract partial record DummyBaseRequest(
    [property: DataMember]
    string BaseValue)
{
    public string AggregateName => "Test";

    public virtual string AggregateId => BaseValue;

    public Metadata CreateMetadata()
    {
        return new Metadata(
                this.CreateMessageMetadata(DateTimeOffset.UtcNow),
                new ContextMetadata(
                    UniqueIdHelper.GenerateUniqueStringId(),
                    "Test user",
                    "PART1",
                    DateTimeOffset.UtcNow.AddMinutes(-1),
                    TimeSpan.FromSeconds(111),
                    1,
                    "ET15",
                    "Test session",
                    ["TestScope"]));
    }
}