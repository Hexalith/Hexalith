// <copyright file="CommandState.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprAggregateActor;

using Ardalis.GuardClauses;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Domain.Abstractions.Messages;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Class CommandState.
/// </summary>
[DataContract]
public class CommandState : MessageState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandState" /> class.
    /// </summary>
    [Obsolete("For serialization only", true)]
    public CommandState()
    {
        Message = null!;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandState"/> class.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="processedDate">The processed date.</param>
    public CommandState(CommandState command, DateTimeOffset processedDate)
        : this(
              Guard.Against.Null(command).ReceivedDate,
              command.IdempotencyId,
              command.Message,
              command.Metadata,
              command.ProcessedDate)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandState" /> class.
    /// </summary>
    /// <param name="receivedDate">The received date.</param>
    /// <param name="processedDate">The processed date.</param>
    /// <param name="idempotencyId">The idempotency identifier.</param>
    /// <param name="message">The command.</param>
    /// <param name="metadata">The metadata.</param>
    [JsonConstructor]
    public CommandState(
        DateTimeOffset receivedDate,
        string idempotencyId,
        BaseCommand message,
        Metadata metadata,
        DateTimeOffset? processedDate)
        : base(receivedDate, idempotencyId, message, metadata)
    {
        Message = message;
        ProcessedDate = processedDate;
    }

    /// <summary>
    /// Gets the command.
    /// </summary>
    /// <value>The command.</value>
    public new BaseCommand Message { get; }

    /// <summary>
    /// Gets the processed date.
    /// </summary>
    /// <value>The processed date.</value>
    public DateTimeOffset? ProcessedDate { get; }
}