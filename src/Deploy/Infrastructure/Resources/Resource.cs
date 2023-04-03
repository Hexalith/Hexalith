// ***********************************************************************
// Assembly         : Deploy
// Author           : Jérôme Piquot
// Created          : 04-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-03-2023
// ***********************************************************************
// <copyright file="Resource.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Deploy.Infrastructure.Resources;

using Azure.ResourceManager;

/// <summary>
/// Class Resource.
/// Implements the <see cref="System.IEquatable{Deploy.Infrastructure.Resources.Resource{TResource}}" />.
/// </summary>
/// <typeparam name="TResource">The type of the t resource.</typeparam>
/// <seealso cref="System.IEquatable{Deploy.Infrastructure.Resources.Resource{TResource}}" />
internal abstract record Resource<TResource>
    where TResource : ArmResource
{
    /// <summary>
    /// Creates the or update asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    public abstract Task<TResource> CreateOrUpdateAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets the resource asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>System.Threading.Tasks.Task&lt;TResource&gt;.</returns>
    public abstract Task<TResource> GetResourceAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets the client.
    /// </summary>
    /// <returns>ArmClient.</returns>
    public abstract ArmClient GetClient();

    /// <summary>
    /// Gets or sets the azure resource.
    /// </summary>
    /// <value>The azure resource.</value>
    protected TResource? AzureResource { get; set; }
}