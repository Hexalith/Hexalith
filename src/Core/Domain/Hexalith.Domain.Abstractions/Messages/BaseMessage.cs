// <copyright file="BaseMessage.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Abstractions.Messages;

using Hexalith.Extensions.Serialization;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Base class for messages.
/// </summary>
[JsonPolymorphicBaseClass]
[DataContract]
public abstract class BaseMessage : IMessage
{
    /// <summary>
    /// Default string used for separating natural keys to compose the aggragate identifier.
    /// </summary>
    protected const string Separator = "-";

    /// <inheritdoc/>
    [IgnoreDataMember]
    [JsonIgnore]
    public string AggregateId => DefaultAggregateId();

    /// <inheritdoc/>
    [IgnoreDataMember]
    [JsonIgnore]
    public string AggregateName => DefaultAggregateName();

    /// <inheritdoc/>
    [IgnoreDataMember]
    [JsonIgnore]
    public int MajorVersion => DefaultMajorVersion();

    /// <inheritdoc/>
    [IgnoreDataMember]
    [JsonIgnore]
    public string MessageName => DefaultMessageName();

    /// <inheritdoc/>
    [IgnoreDataMember]
    [JsonIgnore]
    public int MinorVersion => DefaultMinorVersion();

    /// <summary>
    /// Get the aggregate identifier.
    /// </summary>
    /// <returns>The identifier.</returns>
    protected abstract string DefaultAggregateId();

    /// <summary>
    /// Get the aggregate name.
    /// </summary>
    /// <returns>The name.</returns>
    protected abstract string DefaultAggregateName();

    /// <summary>
    /// Get the message major version.
    /// </summary>
    /// <returns>The major version.</returns>
    protected virtual int DefaultMajorVersion()
    {
        return 0;
    }

    /// <summary>
    /// Get the message name.
    /// </summary>
    /// <returns>The name.</returns>
    protected virtual string DefaultMessageName()
    {
        return GetType().Name;
    }

    /// <summary>
    /// Gets the message minor version.
    /// </summary>
    /// <returns>The minor version.</returns>
    protected virtual int DefaultMinorVersion()
    {
        return 0;
    }
}