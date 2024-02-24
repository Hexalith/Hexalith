// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.AzureCloud
// Author           : Jérôme Piquot
// Created          : 04-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-07-2023
// ***********************************************************************
// <copyright file="ResourceGroupItemBuilder.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.AzureCloud.Builders;

using System.Threading;

using Azure.ResourceManager;
using Azure.ResourceManager.Resources;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class ResourceBuilder.
/// </summary>
/// <typeparam name="TArmResource">The type of the t arm resource.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="ResourceGroupItemBuilder{TArmResource, TData}"/> class.
/// </remarks>
/// <param name="azureBuilder">The azure builder.</param>
/// <param name="resourceGroup">The resource group.</param>
/// <param name="resourceTypeName">Name of the resource type.</param>
/// <param name="uniqueId">The unique identifier.</param>
/// <param name="data">The data.</param>
/// <param name="existing">if set to <c>true</c> [existing].</param>
public abstract class ResourceGroupItemBuilder<TArmResource, TData>(
    AzureBuilder azureBuilder,
    ResourceGroupBuilder resourceGroup,
    string resourceTypeName,
    string uniqueId,
    TData? data,
    bool existing = false) : ResourceBuilder<TArmResource>(azureBuilder, resourceGroup, resourceTypeName, uniqueId, existing)
    where TArmResource : ArmResource
    where TData : class
{
    /// <summary>
    /// Gets the data.
    /// </summary>
    /// <value>The data.</value>
    public TData? Data { get; } = data;

    /// <summary>
    /// Gets the resource group.
    /// </summary>
    /// <value>The resource group.</value>
    public ResourceGroupBuilder ResourceGroup { get; } = resourceGroup;

    /// <inheritdoc/>
    public override async Task<TArmResource> BuildAsync(CancellationToken cancellationToken)
    {
        if (Resource == null)
        {
            ResourceGroupResource group = await ResourceGroup.BuildAsync(cancellationToken).ConfigureAwait(false);
            if (Existing)
            {
                Logger.LogInformation("Getting {ResourceTypeName} {Name} in resource group {ResourceGroup}", ResourceTypeName, UniqueId, group.Data.Name);
                Resource = await GetExistingResourceAsync(cancellationToken).ConfigureAwait(false);
                Logger.LogInformation("{ResourceTypeName} {Name} found", ResourceTypeName, UniqueId);
                return Resource;
            }

            Logger.LogInformation("Creating {ResourceTypeName} {Name} in resource group {ResourceGroup}", ResourceTypeName, UniqueId, group.Data.Name);
            Resource = await CreateOrUpdateResourceAsync(cancellationToken).ConfigureAwait(false);
            Logger.LogInformation("{ResourceTypeName} {Name} created", ResourceTypeName, UniqueId);
        }

        return Resource;
    }
}