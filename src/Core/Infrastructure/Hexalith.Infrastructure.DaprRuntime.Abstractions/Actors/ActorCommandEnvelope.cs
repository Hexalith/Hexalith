// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime
// Author           : jpiquot
// Created          : 02-15-2023
//
// Last Modified By : jpiquot
// Last Modified On : 03-11-2023
// ***********************************************************************
// <copyright file="ActorCommandEnvelope.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Abstractions.Actors;

using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

using Hexalith.Application.Commands;
using Hexalith.Application.Metadatas;

#pragma warning disable IDE0301 // Simplify collection initialization

/// <summary>
/// Actor method object uses Data Contract Serialization that can't serialize and deserialize complex polymorphic objects. This class is used to serialize and deserialize commands and metadatas into JSON strings before the actor proxy serialization.
/// Implements the <see cref="IEquatable{ActorCommandEnvelope}" />.
/// </summary>
/// <seealso cref="IEquatable{ActorCommandEnvelope}" />
[DataContract]
[Serializable]
public class ActorCommandEnvelope : IJsonOnSerializing, IJsonOnDeserialized
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ActorCommandEnvelope" /> class.
    /// </summary>
    /// <param name="commandsJson">The commands json.</param>
    /// <param name="metadatasJson">The metadatas json.</param>
    [Obsolete("Only used for serialization", true)]
    [JsonConstructor]
    public ActorCommandEnvelope(IEnumerable<string> commandsJson, IEnumerable<string> metadatasJson)
    {
        CommandsJson = commandsJson;
        MetadatasJson = metadatasJson;
        Commands = Array.Empty<BaseCommand>();
        Metadatas = Array.Empty<Metadata>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ActorCommandEnvelope" /> class.
    /// </summary>
    [Obsolete("Only used for serialization", true)]
    public ActorCommandEnvelope()
    {
        CommandsJson = Array.Empty<string>();
        MetadatasJson = Array.Empty<string>();
        Commands = Array.Empty<BaseCommand>();
        Metadatas = Array.Empty<Metadata>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ActorCommandEnvelope" /> class.
    /// </summary>
    /// <param name="commands">The commands.</param>
    /// <param name="metadatas">The metadatas.</param>
    /// <exception cref="ArgumentException">Command and Metadata arrays must have the same number of elements. - metadatas.</exception>
    /// <exception cref="ArgumentException">The command array must contain elements. - commands.</exception>
    /// <exception cref="ArgumentException">All commands must be for the same aggregate. - commands.</exception>
    /// <exception cref="ArgumentException">All commands must be for the same aggregate identifier. - commands.</exception>
    public ActorCommandEnvelope(IEnumerable<BaseCommand> commands, IEnumerable<BaseMetadata> metadatas)
    {
        CommandsJson = Array.Empty<string>();
        MetadatasJson = Array.Empty<string>();
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
    /// Gets or sets the command.
    /// </summary>
    /// <value>The command.</value>
    [JsonIgnore]
    [IgnoreDataMember]
    public IEnumerable<BaseCommand> Commands { get; set; }

    /// <summary>
    /// Gets or sets the commands json.
    /// </summary>
    /// <value>The commands json.</value>
    [DataMember(Name = nameof(Commands))]
    [JsonPropertyName(nameof(Commands))]
    public IEnumerable<string> CommandsJson { get; set; }

    /// <summary>
    /// Gets or sets the metadata.
    /// </summary>
    /// <value>The metadata.</value>
    [JsonIgnore]
    [IgnoreDataMember]
    public IEnumerable<BaseMetadata> Metadatas { get; set; }

    /// <summary>
    /// Gets or sets the metadatas json.
    /// </summary>
    /// <value>The metadatas json.</value>
    [DataMember(Name = nameof(Metadatas))]
    [JsonPropertyName(nameof(Metadatas))]
    public IEnumerable<string> MetadatasJson { get; set; }

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
    private void OnDeserialized(StreamingContext context) => OnDeserialized();

    /// <summary>
    /// Called when [serializing].
    /// </summary>
    /// <param name="context">The context.</param>
    [OnSerializing]
    private void OnSerializing(StreamingContext context) => OnSerializing();
}