// <copyright file="MyDummyMessage3.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Domain.Messages;

using Hexalith.PolymorphicSerializations;

[PolymorphicSerialization("MyMessage", 3)]
public partial record MyDummyMessage3(string Id, string Name, int Value)
{
    public string AggregateName => "Dummy";

    public string AggregateId => Id;
}