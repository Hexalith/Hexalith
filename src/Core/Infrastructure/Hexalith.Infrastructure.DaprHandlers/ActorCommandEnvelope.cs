// <copyright file="ActorCommandEnvelope.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprHandlers;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Application.Abstractions.Metadatas;

/// <summary>
/// Class ActorCommandEnvelope.
/// Implements the <see cref="System.IEquatable{Hexalith.Infrastructure.DaprHandlers.ActorCommandEnvelope}" />.
/// </summary>
/// <seealso cref="System.IEquatable{Hexalith.Infrastructure.DaprHandlers.ActorCommandEnvelope}" />
[DataContract]
public record ActorCommandEnvelope
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ActorCommandEnvelope" /> class.
    /// </summary>
    /// <param name="commands">The command.</param>
    /// <param name="metadatas">The metadata.</param>
    [JsonConstructor]
    public ActorCommandEnvelope(BaseCommand[] commands, Metadata[] metadatas)
    {
        Commands = commands;
        Metadatas = metadatas;
        if (Commands.Length != Metadatas.Length)
        {
            throw new ArgumentException("Command and Metadata arrays must have the same number of elements.", nameof(metadatas));
        }

        if (Commands.Length == 0)
        {
            throw new ArgumentException("The command array must contain elements.", nameof(commands));
        }

        foreach (BaseCommand? command in commands.Skip(1))
        {
            if (command.AggregateName != commands[0].AggregateName)
            {
                throw new ArgumentException("All commands must be for the same aggregate.", nameof(commands));
            }

            if (command.AggregateId != commands[0].AggregateId)
            {
                throw new ArgumentException("All commands must be for the same aggregate identifier.", nameof(commands));
            }
        }
    }

    /// <summary>
    /// Gets the command.
    /// </summary>
    /// <value>The command.</value>
    public BaseCommand[] Commands { get; }

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    /// <value>The metadata.</value>
    public Metadata[] Metadatas { get; }
}