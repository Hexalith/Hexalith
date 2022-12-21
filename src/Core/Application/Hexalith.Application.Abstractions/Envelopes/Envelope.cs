// <copyright file="Envelope.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Envelopes;

using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Domain.Abstractions.Messages;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

[DataContract]
public class Envelope : BaseEnvelope<BaseMessage, Metadata>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Envelope"/> class.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="metadata"></param>
    [JsonConstructor]
    public Envelope(BaseMessage message, Metadata metadata)
        : base(message, metadata)
    {
    }
}
