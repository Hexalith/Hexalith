// <copyright file="ActorCommandEnvelope.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprHandlers;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Application.Abstractions.Metadatas;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

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
    /// <param name="command">The command.</param>
    /// <param name="metadata">The metadata.</param>
    [JsonConstructor]
    public ActorCommandEnvelope(BaseCommand command, Metadata metadata)
    {
        Command = command;
        Metadata = metadata;
    }

    /// <summary>
    /// Gets the command.
    /// </summary>
    /// <value>The command.</value>
    public BaseCommand Command { get; }

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    /// <value>The metadata.</value>
    public Metadata Metadata { get; }
}