// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.AzureCloud
// Author           : Jérôme Piquot
// Created          : 04-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-04-2023
// ***********************************************************************
// <copyright file="IResourceBuilder.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.AzureCloud.Builders;

using Azure.ResourceManager;

/// <summary>
/// Interface IResourceBuilder.
/// </summary>
public interface IResourceBuilder
{
    /// <summary>
    /// Gets the azure builder.
    /// </summary>
    /// <value>The azure builder.</value>
    AzureBuilder AzureBuilder { get; }

    /// <summary>
    /// Gets a value indicating whether this <see cref="IResourceBuilder" /> is existing.
    /// </summary>
    /// <value><c>true</c> if existing; otherwise, <c>false</c>.</value>
    public bool Existing { get; }

    /// <summary>
    /// Gets the azure builder.
    /// </summary>
    /// <value>The azure builder.</value>
    IResourceBuilder? Parent { get; }

    /// <summary>
    /// Gets the resource builder existing identifier.
    /// </summary>
    /// <value>The resource builder existing identifier.</value>
    string ResourceBuilderExistingId { get; }

    /// <summary>
    /// Gets the resource builder identifier.
    /// </summary>
    /// <value>The resource builder identifier.</value>
    string ResourceBuilderId { get; }

    /// <summary>
    /// Gets the resource builder not existing identifier.
    /// </summary>
    /// <value>The resource builder not existing identifier.</value>
    string ResourceBuilderNotExistingId { get; }

    /// <summary>
    /// Gets the unique identifier.
    /// </summary>
    /// <value>The unique identifier.</value>
    string UniqueId { get; }

    /// <summary>
    /// Builds the asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;ArmResource&gt;.</returns>
    Task<ArmResource> BuildAsync(CancellationToken cancellationToken);
}