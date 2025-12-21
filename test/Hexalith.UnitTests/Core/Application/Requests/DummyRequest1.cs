// <copyright file="DummyRequest1.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Requests;

using System.Runtime.Serialization;

using Hexalith.Commons.Strings;
using Hexalith.Extensions.Helpers;
using Hexalith.PolymorphicSerializations;

[PolymorphicSerialization]
public partial record DummyRequest1(
    string BaseValue,
    [property: DataMember]
    int Value1) : DummyBaseRequest(BaseValue)
{
    public override string AggregateId => base.AggregateId + "-" + Value1.ToInvariantString();
}