// <copyright file="DummyMessage2.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Domain.Messages;

using Hexalith.Extensions.Helpers;
using Hexalith.PolymorphicSerialization;

[PolymorphicSerialization]
public partial record DummyMessage2(string BaseValue, int Value2) : DummyBaseMessage(BaseValue)
{
    public override string AggregateId => base.AggregateId + "-" + Value2.ToInvariantString();
}