// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-06-2023
// ***********************************************************************
// <copyright file="Metadata.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Metadatas;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.Messages;
using Hexalith.Extensions;
using Hexalith.Extensions.Helpers;

#pragma warning disable CA1724 // Type names should not match namespaces

/// <summary>
/// Class Metadata.
/// Implements the <see cref="IMetadata" />.
/// </summary>
/// <seealso cref="IMetadata" />
[DataContract]
public class Metadata : BaseMetadata
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Metadata" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public Metadata()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Metadata" /> class.
    /// </summary>
    /// <param name="version">The version.</param>
    /// <param name="message">The message.</param>
    /// <param name="context">The context.</param>
    /// <param name="scopes">The scopes.</param>
    [JsonConstructor]
    public Metadata(
        MessageMetadata message,
        ContextMetadata context,
        IEnumerable<string>? scopes)
        : base(message, context, scopes)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Metadata" /> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="message">The message.</param>
    /// <param name="date">The date.</param>
    /// <param name="context">The context.</param>
    /// <param name="scopes">The scopes.</param>
    public Metadata(
        string id,
        IMessage message,
        DateTimeOffset date,
        ContextMetadata context,
        IEnumerable<string>? scopes)
        : base(id, message, date, context, scopes)
    {
    }

    /// <summary>
    /// Creates the new.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="metadata">The metadata.</param>
    /// <param name="date">The date.</param>
    /// <returns>Metadata.</returns>
    public static Metadata CreateNew([NotNull] IMessage message, [NotNull] IMetadata metadata, DateTimeOffset date)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(metadata);
        return new Metadata(
            UniqueIdHelper.GenerateUniqueStringId(),
            message,
            date,
            new(metadata.Context),
            metadata.Scopes);
    }

    /// <summary>
    /// Creates the new.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="metadata">The metadata.</param>
    /// <returns>Metadata.</returns>
    public static Metadata CreateNew([NotNull] IMessage message, [NotNull] IMetadata metadata)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(metadata);
        return new Metadata(
            UniqueIdHelper.GenerateUniqueStringId(),
            message,
            metadata.Message.CreatedDate,
            new(metadata.Context),
            metadata.Scopes);
    }
}