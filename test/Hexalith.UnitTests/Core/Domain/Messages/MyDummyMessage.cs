// <copyright file="MyDummyMessage.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Domain.Messages;

using Hexalith.PolymorphicSerializations;

[PolymorphicSerialization]
public partial record MyDummyMessage(string Id, string Name, int Value)
{
    public string DomainName => "Dummy";

    public string DomainId => Id;
}