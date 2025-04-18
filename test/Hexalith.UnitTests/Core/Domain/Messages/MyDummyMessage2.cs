// <copyright file="MyDummyMessage2.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Domain.Messages;

using Hexalith.PolymorphicSerializations;

[PolymorphicSerialization(version: 2)]
public partial record MyDummyMessage2(string Id, string Name, int Value)
{
    public string AggregateName => "Dummy";

    public string AggregateId => Id;
}