// <copyright file="CommandDispatchDoEvent.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Commands;

using Hexalith.PolymorphicSerializations;

[PolymorphicSerialization]
public partial record CommandDispatchDoEvent
{
    public string DefaultDomainId => "123";

    public string DefaultDomainName => "Test";
}