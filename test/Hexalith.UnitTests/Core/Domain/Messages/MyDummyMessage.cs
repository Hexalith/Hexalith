// <copyright file="MyDummyMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.UnitTests.Core.Domain.Messages;

using Hexalith.PolymorphicSerialization;

[PolymorphicSerialization]
public partial record MyDummyMessage(string Id, string Name, int Value)
{
    public string AggregateName => "Dummy";

    public string AggregateId => Id;
}