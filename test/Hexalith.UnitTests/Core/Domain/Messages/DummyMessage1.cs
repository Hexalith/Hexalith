// <copyright file="DummyMessage1.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Domain.Messages;

using Hexalith.Extensions.Helpers;
using Hexalith.PolymorphicSerializations;

[PolymorphicSerialization]
public partial record DummyMessage1(string BaseValue, int Value1) : DummyBaseMessage(BaseValue)
{
    public override string AggregateId => base.AggregateId + "-" + Value1.ToInvariantString();
}