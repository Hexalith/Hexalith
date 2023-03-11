// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime
// Author           : Jérôme Piquot
// Created          : 02-15-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 03-10-2023
// ***********************************************************************
// <copyright file="ActorCommandEnvelope.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Handlers;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Application.Abstractions.Metadatas;

/// <summary>
/// Class ActorCommandEnvelope.
/// Implements the <see cref="IEquatable{ActorCommandEnvelope}" />.
/// </summary>
/// <seealso cref="IEquatable{ActorCommandEnvelope}" />
[DataContract]
public record ActorCommandEnvelope
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ActorCommandEnvelope" /> class.
    /// </summary>
    /// <param name="commands">The command.</param>
    /// <param name="metadatas">The metadata.</param>
    /// <exception cref="ArgumentException">Command and Metadata arrays must have the same number of elements. - metadatas.</exception>
    /// <exception cref="ArgumentException">The command array must contain elements. - commands.</exception>
    /// <exception cref="ArgumentException">All commands must be for the same aggregate. - commands.</exception>
    /// <exception cref="ArgumentException">All commands must be for the same aggregate identifier. - commands.</exception>
    [JsonConstructor]
    public ActorCommandEnvelope(IEnumerable<BaseCommand> commands, IEnumerable<BaseMetadata> metadatas)
    {
        Commands = commands;
        Metadatas = metadatas;
        BaseCommand[] cmds = Commands.ToArray();
        BaseMetadata[] metas = Metadatas.ToArray();
        if (cmds.Length != metas.Length)
        {
            throw new ArgumentException("Command and Metadata arrays must have the same number of elements.", nameof(metadatas));
        }

        if (cmds.Length == 0)
        {
            throw new ArgumentException("The command array must contain elements.", nameof(commands));
        }

        foreach (BaseCommand? command in cmds.Skip(1))
        {
            if (command.AggregateName != cmds[0].AggregateName)
            {
                throw new ArgumentException("All commands must be for the same aggregate.", nameof(commands));
            }

            if (command.AggregateId != cmds[0].AggregateId)
            {
                throw new ArgumentException("All commands must be for the same aggregate identifier.", nameof(commands));
            }
        }
    }

    /// <summary>
    /// Gets the command.
    /// </summary>
    /// <value>The command.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public IEnumerable<BaseCommand> Commands { get; }

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    /// <value>The metadata.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public IEnumerable<BaseMetadata> Metadatas { get; }
}