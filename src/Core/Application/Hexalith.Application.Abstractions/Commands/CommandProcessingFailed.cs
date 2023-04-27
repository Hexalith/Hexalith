// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-30-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-30-2023
// ***********************************************************************
// <copyright file="CommandProcessingFailed.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Abstractions.Commands;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Abstractions.Notifications;
using Hexalith.Application.Abstractions.Tasks;

/// <summary>
/// Class CommandProcessingStalled.
/// Implements the <see cref="Hexalith.Application.Abstractions.Commands.BaseCommand" />.
/// </summary>
/// <seealso cref="Hexalith.Application.Abstractions.Commands.BaseCommand" />
[DataContract]
public class CommandProcessingFailed : BaseNotification
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandProcessingFailed" /> class.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="taskProcessor">The task processor.</param>
    [JsonConstructor]
    public CommandProcessingFailed(BaseCommand command, TaskProcessor taskProcessor)
        : base(
            $"Command {command.TypeName} failed.",
            taskProcessor.Failure?.Message ?? string.Empty,
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
    protected override string DefaultAggregateId() => Command.AggregateId;

    /// <inheritdoc/>
    protected override string DefaultAggregateName() => Command.AggregateName;
}