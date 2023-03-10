// ***********************************************************************
// Assembly         : Hexalith.Domain.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 03-10-2023
// ***********************************************************************
// <copyright file="BaseMessage.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Abstractions.Messages;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.Abstractions.Converters;
using Hexalith.Extensions.Serialization;

/// <summary>
/// Base class for messages.
/// </summary>
[DataContract]
[JsonConverter(typeof(PolymorphicJsonConverter<BaseMessage>))]
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
    public int MajorVersion => DefaultMajorVersion();

    /// <inheritdoc/>
    [IgnoreDataMember]
    [JsonIgnore]
    public int MinorVersion => DefaultMinorVersion();

    /// <inheritdoc/>
    [IgnoreDataMember]
    [JsonIgnore]
    public string TypeName => DefaultTypeName();

    /// <summary>
    /// Get the aggregate identifier.
    /// </summary>
    /// <returns>The identifier.</returns>
    protected virtual string DefaultAggregateId()
    {
        return string.Empty;
    }

    /// <summary>
    /// Get the aggregate name.
    /// </summary>
    /// <returns>The name.</returns>
    protected virtual string DefaultAggregateName()
    {
        return GetType().Name;
    }

    /// <summary>
    /// Get the message major version.
    /// </summary>
    /// <returns>The major version.</returns>
    protected virtual int DefaultMajorVersion()
    {
        return 0;
    }

    /// <summary>
    /// Gets the message minor version.
    /// </summary>
    /// <returns>The minor version.</returns>
    protected virtual int DefaultMinorVersion()
    {
        return 0;
    }

    /// <summary>
    /// Get the message name.
    /// </summary>
    /// <returns>The name.</returns>
    protected virtual string DefaultTypeName()
    {
        return GetType().Name;
    }
}