// <copyright file="DummyMessage.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Metadatas;

using Hexalith.Commons.Strings;
using Hexalith.Extensions.Helpers;
using Hexalith.PolymorphicSerializations;

public record DummyMessage(int Value1, string Value2) : Polymorphic
{
    public string DomainId => Value1.ToInvariantString();

    public static string DomainName => "Dummy";
}