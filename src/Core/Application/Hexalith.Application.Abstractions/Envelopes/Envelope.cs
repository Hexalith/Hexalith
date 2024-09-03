// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-06-2023
// ***********************************************************************
// <copyright file="Envelope.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Envelopes;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Metadatas;
using Hexalith.Domain.Messages;
using Hexalith.Extensions;

/// <summary>
/// Class Envelope.
/// Implements the <see cref="BaseEnvelope{BaseMessage, Metadata}" />.
/// </summary>
/// <seealso cref="BaseEnvelope{BaseMessage, Metadata}" />
[DataContract]
[Serializable]
public class Envelope : BaseEnvelope<BaseMessage, BaseMetadata>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Envelope" /> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="metadata">The metadata.</param>
    [JsonConstructor]
    public Envelope(BaseMessage message, BaseMetadata metadata)
        : base(message, metadata)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Envelope"/> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public Envelope()
        : base(new BaseMessage(), new Metadata())
    {
    }
}