// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 09-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-12-2023
// ***********************************************************************
// <copyright file="IMessageMetadata.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Metadatas;

/// <summary>
/// The message metadata.
/// </summary>
public interface IMessageMetadata
{
    /// <summary>
    /// Gets the aggregate.
    /// </summary>
    /// <value>The aggregate.</value>
    IAggregateMetadata Aggregate { get; }

    /// <summary>
    /// Gets the created date.
    /// </summary>
    /// <value>The created date.</value>
    DateTimeOffset CreatedDate { get; }

    /// <summary>
    /// Gets the aggregate identifier.
    /// </summary>
    /// <value>The identifier.</value>
    string Id { get; }

    /// <summary>
    /// Gets the aggregate name.
    /// </summary>
    /// <value>The name.</value>
    string Name { get; }

    /// <summary>
    /// Gets the message version.
    /// </summary>
    /// <value>The version.</value>
    IMessageVersion Version { get; }
}