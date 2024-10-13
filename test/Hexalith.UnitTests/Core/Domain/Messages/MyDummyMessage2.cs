// <copyright file="MyDummyMessage2.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.UnitTests.Core.Domain.Messages;

using Hexalith.PolymorphicSerialization;

[PolymorphicSerialization(version: 2)]
public partial record MyDummyMessage2(string Id, string Name, int Value)
{
    public string AggregateName => "Dummy";

    public string AggregateId => Id;
}