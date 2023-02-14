// <copyright file="ActorCommandEnvelope.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprHandlers;

using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Infrastructure.Serialization.Helpers;

/// <summary>
/// Class ActorCommandEnvelope.
/// Implements the <see cref="System.IEquatable{Hexalith.Infrastructure.DaprHandlers.ActorCommandEnvelope}" />.
/// </summary>
/// <seealso cref="System.IEquatable{Hexalith.Infrastructure.DaprHandlers.ActorCommandEnvelope}" />
[DataContract]
public class ActorCommandEnvelope : IJsonOnSerializing, IJsonOnDeserialized
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ActorCommandEnvelope" /> class.
    /// </summary>
    /// <param name="commands">The command.</param>
    /// <param name="metadatas">The metadata.</param>
    [Obsolete("Only used for serialization", true)]
    [JsonConstructor]
    public ActorCommandEnvelope(string[]? commandsJson, string[]? metadatasJson)
    {
        CommandsJson = commandsJson;
        MetadatasJson = metadatasJson;
    }

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

    [OnSerializing]
    public void OnSerializing()
    {
        var options = new JsonSerializerOptions().AddPolymorphism();
        CommandsJson = Commands.Select (p => JsonSerializer.Serialize(p, options)).ToArray();
        MetadatasJson = Metadatas.Select(p => JsonSerializer.Serialize(p, options)).ToArray();
    }

    [OnDeserialized]
    public void OnDeserialized()
    {
        var options = new JsonSerializerOptions().AddPolymorphism();
        Commands = CommandsJson == null
            ? Array.Empty<BaseCommand>()
            : CommandsJson.Select(p => JsonSerializer.Deserialize<BaseCommand>(p, options)!).ToArray();
        Metadatas = MetadatasJson == null
            ? Array.Empty<Metadata>()
            : MetadatasJson.Select(p => JsonSerializer.Deserialize<Metadata>(p, options)!).ToArray();
    }

    /// <summary>
    /// Gets the command.
    /// </summary>
    /// <value>The command.</value>
    [JsonIgnore]
    [IgnoreDataMember]
    public BaseCommand[] Commands { get; private set; }

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    /// <value>The metadata.</value>
    [JsonIgnore]
    [IgnoreDataMember]
    public Metadata[] Metadatas { get; private set; }

    [DataMember(Name = nameof(Commands))]
    [JsonPropertyName(nameof(Commands))]
    public string[]? CommandsJson { get; private set;}

    [DataMember(Name = nameof(Metadatas))]
    [JsonPropertyName(nameof(Metadatas))]
    public string[]? MetadatasJson { get; private set; }
}