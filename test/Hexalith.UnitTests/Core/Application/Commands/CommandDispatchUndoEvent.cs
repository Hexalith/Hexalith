// <copyright file="CommandDispatchUndoEvent.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Commands;

using Hexalith.PolymorphicSerialization;

[PolymorphicSerialization]
public partial record CommandDispatchUndoEvent
{
    public string DefaultAggregateId => "123";

    public string DefaultAggregateName => "Test";
}