// <copyright file="DummyCommand2.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Commands;

using System.Runtime.Serialization;

using Hexalith.Extensions.Helpers;
using Hexalith.PolymorphicSerialization;

[PolymorphicSerialization]
public partial record DummyCommand2(
        string BaseValue,
        [property: DataMember]
        int Value2) : DummyBaseCommand(BaseValue)
{
    public static DummyCommand2 Create() => new("Test123", 35453);

    public override string AggregateId => BaseValue + "-" + Value2.ToInvariantString();
}