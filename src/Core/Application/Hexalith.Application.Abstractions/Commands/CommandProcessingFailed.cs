// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-30-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-30-2023
// ***********************************************************************
// <copyright file="CommandProcessingFailed.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Commands;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Tasks;
using Hexalith.Domain.Notifications;

/// <summary>
/// Class CommandProcessingStalled.
/// Implements the <see cref="BaseCommand" />.
/// </summary>
/// <seealso cref="BaseCommand" />
[DataContract]
public class CommandProcessingFailed : BaseNotification
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandProcessingFailed"/> class.
    /// </summary>
    /// <param name="correlationId">The correlation identifier.</param>
    /// <param name="command">The command.</param>
    /// <param name="taskProcessor">The task processor.</param>
    [JsonConstructor]
    public CommandProcessingFailed(string correlationId, BaseCommand command, TaskProcessor taskProcessor)
        : base(
            (command ?? throw new ArgumentNullException(nameof(command))).AggregateName,
            command.AggregateId,
            $"Command {command.TypeName} failed.",
            (taskProcessor ?? throw new ArgumentNullException(nameof(taskProcessor))).Failure?.Message ?? string.Empty,
            NotificationSeverity.Error,
            taskProcessor.Failure?.TechnicalError ?? string.Empty)
    {
        Command = command;
        TaskProcessor = taskProcessor;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandProcessingFailed" /> class.
    /// </summary>
    [Obsolete("For serialization purpose only.", true)]
    public CommandProcessingFailed()
    {
        Command = null!;
        TaskProcessor = null!;
    }

    /// <summary>
    /// Gets the command.
    /// </summary>
    /// <value>The command.</value>
    public BaseCommand Command { get; }

    /// <summary>
    /// Gets the task processor.
    /// </summary>
    /// <value>The task processor.</value>
    public TaskProcessor TaskProcessor { get; }

    /// <inheritdoc/>
    protected override string DefaultAggregateId()
        => Command.AggregateId;

    /// <inheritdoc/>
    protected override string DefaultAggregateName()
        => Command.AggregateName;
}