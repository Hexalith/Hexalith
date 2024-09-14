// <copyright file="DummyMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Metadatas;

using Hexalith.Extensions.Helpers;
using Hexalith.PolymorphicSerialization;

[PolymorphicSerialization]
public partial record DummyMessage(int Value1, string Value2) : PolymorphicRecordBase
{
    public string AggregateId => Value1.ToInvariantString();

    public static string AggregateName => "Dummy";
}