﻿// <copyright file="DummyCommand1.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Commands;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Metadatas;
using Hexalith.Extensions.Helpers;

public class DummyCommand1 : DummyBaseCommand
{
    private static readonly string[] _scopes = ["sc01", "sc02"];

    [JsonConstructor]
    public DummyCommand1(string baseValue, int value1)
        : base(baseValue) => Value1 = value1;

    public DummyCommand1()
    {
    }

    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public int Value1 { get; set; }

    public static DummyCommand1 Create() => new("Test123", 35453);

    public new Metadata CreateMetadata()
    {
        return new Metadata(
            UniqueIdHelper.GenerateUniqueStringId(),
            this,
            DateTimeOffset.Now,
            new ContextMetadata(
                "COR424202",
                "TestUser1",
                DateTimeOffset.UtcNow,
                10,
                "SES2132"),
            _scopes);
    }

    protected override string DefaultAggregateId() => BaseValue + "-" + Value1.ToInvariantString();

    protected override int DefaultMajorVersion() => 4;

    protected override int DefaultMinorVersion() => 6;
}