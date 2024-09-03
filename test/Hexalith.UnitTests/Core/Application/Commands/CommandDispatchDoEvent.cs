﻿// <copyright file="CommandDispatchDoEvent.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Commands;

using Hexalith.Domain.Events;

internal sealed class CommandDispatchDoEvent : BaseEvent
{
    public CommandDispatchDoEvent()
    {
    }

    protected override string DefaultAggregateId() => "123";

    protected override string DefaultAggregateName() => "Test";
}