// <copyright file="DummyCommand1.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Commands;

using System.Runtime.Serialization;

using Hexalith.Application.Metadatas;
using Hexalith.Extensions.Helpers;
using Hexalith.PolymorphicSerialization;

[PolymorphicSerialization]
public partial record DummyCommand1(
    string BaseValue,
    [property: DataMember]
    int Value1) : DummyBaseCommand(BaseValue)
{
    private static readonly string[] _scopes = ["sc01", "sc02"];

    public static DummyCommand1 Create() => new("Test123", 35453);

    public new Metadata CreateMetadata()
    {
        return new Metadata(
            new MessageMetadata(this, DateTimeOffset.UtcNow),
            new ContextMetadata(
                "COR424202",
                "TestUser1",
                "PART22",
                DateTimeOffset.UtcNow,
                10,
                "SES2132",
                _scopes));
    }

    public override string AggregateId => BaseValue + "-" + Value1.ToInvariantString();
}