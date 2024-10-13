// <copyright file="MyDummyMessage3.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.UnitTests.Core.Domain.Messages;

using Hexalith.PolymorphicSerialization;

[PolymorphicSerialization("MyMessage", 3)]
public partial record MyDummyMessage3(string Id, string Name, int Value)
{
    public string AggregateName => "Dummy";

    public string AggregateId => Id;
}