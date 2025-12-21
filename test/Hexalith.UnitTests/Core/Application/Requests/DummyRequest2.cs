// <copyright file="DummyRequest2.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Requests;

using Hexalith.Commons.Strings;
using Hexalith.Extensions.Helpers;
using Hexalith.PolymorphicSerializations;

[PolymorphicSerialization]
public partial record DummyRequest2(string BaseValue, int Value2) : DummyBaseRequest(BaseValue)
{
    public override string DomainId => base.DomainId + "-" + Value2.ToInvariantString();
}