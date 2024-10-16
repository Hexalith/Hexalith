﻿// <copyright file="DummyEvent1.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Domain.Events;

using Hexalith.Extensions.Helpers;
using Hexalith.PolymorphicSerialization;

[PolymorphicSerialization]
public partial record DummyEvent1(string BaseValue, int Value1) : DummyBaseEvent(BaseValue)
{
    public override string AggregateId => base.AggregateId + "-" + Value1.ToInvariantString();
}