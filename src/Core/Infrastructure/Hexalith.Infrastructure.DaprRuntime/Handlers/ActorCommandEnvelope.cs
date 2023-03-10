// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime
// Author           : jpiquot
// Created          : 02-15-2023
//
// Last Modified By : jpiquot
// Last Modified On : 02-14-2023
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
using System.Text.Json;
using System.Text.Json.Serialization;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Application.Abstractions.Metadatas;

/// <summary>
/// Class ActorCommandEnvelope.
/// Implements the <see cref="IEquatable{ActorCommandEnvelope}" />.
/// </summary>
/// <seealso cref="IEquatable{ActorCommandEnvelope}" />
[DataContract]
public class ActorCommandEnvelope : IJsonOnSerializing, IJsonOnDeserialized
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ActorCommandEnvelope" /> class.
    /// </summary>
    /// <param name="commandsJson">The commands json.</param>
    /// <param name="metadatasJson">The metadatas json.</param>
    [Obsolete("Only used for serialization", true)]
    [JsonConstructor]
    public ActorCommandEnvelope(string[]? commandsJson, string[]? metadatasJson)
    {
        CommandsJson = commandsJson;
        MetadatasJson = metadatasJson;
        Commands = Array.Empty<BaseCommand>();
        Metadatas = Array.Empty<Metadata>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ActorCommandEnvelope"/> class.
    /// </summary>
    /// <param name="commands">The commands.</param>
    /// <param name="metadatas">The metadatas.</param>
    /// <exception cref="ArgumentException">Command and Metadata arrays must have the same number of elements. - metadatas.</exception>
    /// <exception cref="ArgumentException">The command array must contain elements. - commands.</exception>
    /// <exception cref="ArgumentException">All commands must be for the same aggregate. - commands.</exception>
    /// <exception cref="ArgumentException">All commands must be for the same aggregate identifier. - commands.</exception>
    public ActorCommandEnvelope(BaseCommand[] commands, BaseMetadata[] metadatas)
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
    [JsonIgnore]
    [IgnoreDataMember]
    public BaseCommand[] Commands { get; private set; }

    /// <summary>
    /// Gets the commands json.
    /// </summary>
    /// <value>The commands json.</value>
    [DataMember(Name = nameof(Commands))]
    [JsonPropertyName(nameof(Commands))]
    public string[]? CommandsJson { get; private set; }

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    /// <value>The metadata.</value>
    [JsonIgnore]
    [IgnoreDataMember]
    public BaseMetadata[] Metadatas { get; private set; }

    /// <summary>
    /// Gets the metadatas json.
    /// </summary>
    /// <value>The metadatas json.</value>
    [DataMember(Name = nameof(Metadatas))]
    [JsonPropertyName(nameof(Metadatas))]
    public string[]? MetadatasJson { get; private set; }

    /// <summary>
    /// The method that is called after deserialization.
    /// </summary>
    public void OnDeserialized()
    {
        Commands = CommandsJson == null
            ? Array.Empty<BaseCommand>()
            : CommandsJson.Select(p => JsonSerializer.Deserialize<BaseCommand>(p)!).ToArray();
        Metadatas = MetadatasJson == null
            ? Array.Empty<Metadata>()
            : MetadatasJson.Select(p => JsonSerializer.Deserialize<Metadata>(p)!).ToArray();
    }

    /// <summary>
    /// The method that is called before serialization.
    /// </summary>
    public void OnSerializing()
    {
        CommandsJson = Commands.Select(p => JsonSerializer.Serialize(p)).ToArray();
        MetadatasJson = Metadatas.Select(p => JsonSerializer.Serialize(p)).ToArray();
    }

    /// <summary>
    /// Called when [deserialized].
    /// </summary>
    /// <param name="context">The context.</param>
    [OnDeserialized]
    private void OnDeserialized(StreamingContext context)
    {
        OnDeserialized();
    }

    /// <summary>
    /// Called when [serializing].
    /// </summary>
    /// <param name="context">The context.</param>
    [OnSerializing]
    private void OnSerializing(StreamingContext context)
    {
        OnSerializing();
    }
}