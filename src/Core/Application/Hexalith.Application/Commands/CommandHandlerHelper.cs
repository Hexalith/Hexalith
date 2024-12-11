// <copyright file="CommandHandlerHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Commands;

using System;

using Hexalith.Application.Events;
using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Domain.Aggregates;
using Hexalith.PolymorphicSerialization;

/// <summary>
/// Provides helper methods for handling commands.
/// </summary>
public static class CommandHandlerHelper
{
    /// <summary>
    /// Creates the command result based on the apply result.
    /// </summary>
    /// <param name="result">The result of applying the command.</param>
    /// <param name="ev">The event associated with the command.</param>
    /// <param name="metadata">The metadata associated with the event.</param>
    /// <param name="timeProvider">The time provider to get the current time.</param>
    /// <returns>The result of executing the command.</returns>
    public static ExecuteCommandResult CreateCommandResult(this ApplyResult result, object ev, object metadata, TimeProvider timeProvider)
    {
        return result.Failed
                ? new ExecuteCommandResult(
                    result.Aggregate,
                    [],
                    [
                        new DomainEventCancelled(
                            result.Reason ?? "Unknown reason",
                            new MessageState((PolymorphicRecordBase)ev, Metadata.CreateNew(ev, (Metadata)metadata, timeProvider.GetLocalNow()))),
                    ])
                : new ExecuteCommandResult(result.Aggregate, [ev], result.Messages);
    }
}