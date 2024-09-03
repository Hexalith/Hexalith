// <copyright file="MessageVersion.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Metadatas;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;

/// <summary>
/// The message version.
/// </summary>
[DataContract]
public class MessageVersion : IMessageVersion
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MessageVersion"/> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public MessageVersion()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageVersion"/> class.
    /// </summary>
    /// <param name="major">The major.</param>
    /// <param name="minor">The minor.</param>
    [JsonConstructor]
    public MessageVersion(int major, int minor)
    {
        Major = major;
        Minor = minor;
    }

    /// <summary>
    /// Gets the major version.
    /// </summary>
    /// <value>The major.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public int Major { get; }

    /// <summary>
    /// Gets the minor version.
    /// </summary>
    /// <value>The minor.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public int Minor { get; }
}