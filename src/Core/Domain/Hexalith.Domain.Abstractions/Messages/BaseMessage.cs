// <copyright file="BaseMessage.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Messages;

using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions.Serialization;

/// <summary>
/// Base class for messages.
/// </summary>
[DataContract]
[Serializable]
[JsonConverter(typeof(PolymorphicJsonConverter<BaseMessage>))]
[DebuggerDisplay("{AggregateName}/{AggregateId}/{TypeName}/v{MajorVersion}/{MinorVersion}")]
[Obsolete]
public class BaseMessage : IMessage, IPolymorphicSerializable
{
    /// <summary>
    /// Default string used for separating natural keys to compose the aggregate identifier.
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
    public bool IsPrivateToAggregate => DefaultIsPrivateToAggregate();

    /// <inheritdoc/>
    [IgnoreDataMember]
    [JsonIgnore]
    public int MajorVersion => DefaultMajorVersion();

    /// <inheritdoc/>
    [IgnoreDataMember]
    [JsonIgnore]
    public int MinorVersion => DefaultMinorVersion();

    /// <inheritdoc/>
    public string TypeMapName => IPolymorphicSerializable.GetTypeMapName(TypeName, MajorVersion, MinorVersion);

    /// <inheritdoc/>
    [IgnoreDataMember]
    [JsonIgnore]
    [Obsolete]
    public string TypeName => DefaultTypeName();

    /// <summary>
    /// Get the aggregate identifier.
    /// </summary>
    /// <returns>The identifier.</returns>
    protected virtual string DefaultAggregateId() => DefaultAggregateName();

    /// <summary>
    /// Get the aggregate name.
    /// </summary>
    /// <returns>The name.</returns>
    protected virtual string DefaultAggregateName() => string.Empty;

    /// <summary>
    /// Defaults the is private to aggregate.
    /// </summary>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    protected virtual bool DefaultIsPrivateToAggregate() => false;

    /// <summary>
    /// Get the message major version.
    /// </summary>
    /// <returns>The major version.</returns>
    protected virtual int DefaultMajorVersion() => 0;

    /// <summary>
    /// Gets the message minor version.
    /// </summary>
    /// <returns>The minor version.</returns>
    protected virtual int DefaultMinorVersion() => 0;

    /// <summary>
    /// Get the message name.
    /// </summary>
    /// <returns>The name.</returns>
    protected virtual string DefaultTypeName() => GetType().Name;
}