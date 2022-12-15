// <copyright file="Metatada.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Metadatas;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

[DataContract]
public class Metatada : IMetadata
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Metatada"/> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    public Metatada()
    {
        Message = new MessageMetadata();
        Version = new MetadataVersion();
        Context = new ContextMetadata();
        Date = DateTimeOffset.MinValue;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Metatada"/> class.
    /// </summary>
    /// <param name="version"></param>
    /// <param name="message"></param>
    /// <param name="context"></param>
    /// <param name="date"></param>
    /// <param name="scopes"></param>
    [JsonConstructor]
    public Metatada(
        IMetadataVersion version,
        IMessageMetadata message,
        IContextMetadata context,
        DateTimeOffset date,
        IEnumerable<string>? scopes)
    {
        Version = version;
        Message = message;
        Context = context;
        Date = date;
        Scopes = scopes;
    }

    /// <inheritdoc/>
    public IContextMetadata Context { get; private set; }

    /// <inheritdoc/>
    public DateTimeOffset Date { get; private set; }

    /// <inheritdoc/>
    public IMessageMetadata Message { get; private set; }

    /// <inheritdoc/>
    public IEnumerable<string>? Scopes { get; private set; }

    /// <inheritdoc/>
    public IMetadataVersion Version { get; private set; }
}