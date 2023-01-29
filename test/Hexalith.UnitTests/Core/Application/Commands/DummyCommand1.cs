// <copyright file="DummyCommand1.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Commands;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Extensions.Helpers;
using Hexalith.UnitTests.Core.Domain;

[DataContract]
public class DummyCommand1 : DummyBaseCommand
{
    [JsonConstructor]
    public DummyCommand1(string baseValue, int value1)
        : base(baseValue)
    {
        Value1 = value1;
    }

    public DummyCommand1()
    {
    }

    public int Value1 { get; }

    public static DummyCommand1 Create()
    {
        return new DummyCommand1("Test123", 35453);
    }

    public Metadata CreateMetadata()
    {
        return new Metadata(
            "ID35433",
            this,
            DateTimeOffset.Now,
            new ContextMetadata(
                "COR424202",
                "TestUser1",
                DateTimeOffset.UtcNow,
                10,
                "SES2132"),
            new[] { "sc01", "sc02" });
    }

    protected override string DefaultAggregateId()
    {
        return BaseValue + "-" + Value1.ToInvariantString();
    }
}