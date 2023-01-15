// <copyright file="MessageVersion.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Metadatas;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// The message version.
/// </summary>
[DataContract]
public class MessageVersion : IMessageVersion
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MessageVersion"/> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
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