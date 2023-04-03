// ***********************************************************************
// Assembly         : Deploy
// Author           : Jérôme Piquot
// Created          : 04-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-02-2023
// ***********************************************************************
// <copyright file="ContainerRegistry.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Deploy.Infrastructure.Resources;

using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.ContainerRegistry;
using Azure.ResourceManager.ContainerRegistry.Models;
using Azure.ResourceManager.Resources;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class ContainerRegistry.
/// Implements the <see cref="Deploy.Infrastructure.Resources.Resource{Azure.ResourceManager.Resources.ContainerRegistryResource}" />
/// Implements the <see cref="System.IEquatable{Deploy.Infrastructure.Resources.Resource{Azure.ResourceManager.Resources.ContainerRegistryResource}}" />
/// Implements the <see cref="System.IEquatable{Deploy.Infrastructure.Resources.ContainerRegistry}" />.
/// </summary>
/// <seealso cref="Deploy.Infrastructure.Resources.Resource{Azure.ResourceManager.Resources.ContainerRegistryResource}" />
/// <seealso cref="System.IEquatable{Deploy.Infrastructure.Resources.Resource{Azure.ResourceManager.Resources.ContainerRegistryResource}}" />
/// <seealso cref="System.IEquatable{Deploy.Infrastructure.Resources.ContainerRegistry}" />
internal record ContainerRegistry(ResourceGroup ResourceGroup, string Name, string? Location, string? Sku, ILogger Logger)
    : Resource<ContainerRegistryResource>(Logger)
{
    /// <inheritdoc/>
    public override async Task<ContainerRegistryResource> CreateOrUpdateAsync(CancellationToken cancellationToken)
    {
        ResourceGroupResource group = await ResourceGroup.GetResourceAsync(cancellationToken);
        ContainerRegistryCollection registries = group.GetContainerRegistries();
        _ = string.IsNullOrWhiteSpace(Sku) ? ContainerRegistrySkuName.Basic : new ContainerRegistrySkuName(Sku);
        AzureLocation location = string.IsNullOrWhiteSpace(Location) ? group.Data.Location : Location;
        ContainerRegistryData resourceGroupData = new(location, new ContainerRegistrySku(ContainerRegistrySkuName.Basic));
        Logger.LogInformation("Creating container registry {Name} in resource group {ResourceGroup} in location {Location} with sku {Sku}", Name, ResourceGroup.Name, Location, Sku);
        ArmOperation<ContainerRegistryResource> operation = await registries
            .CreateOrUpdateAsync(
                Azure.WaitUntil.Completed,
                Name,
                resourceGroupData,
                cancellationToken);
        Logger.LogInformation("Container registry {Name} created", Name);
        return operation.Value;
    }

    /// <inheritdoc/>
    public override async Task<ContainerRegistryResource> GetResourceAsync(CancellationToken cancellationToken)
    {
        if (AzureResource == null)
        {
            ResourceGroupResource group = await ResourceGroup.GetResourceAsync(cancellationToken);
            AzureResource = await group.GetContainerRegistryAsync(Name, cancellationToken);
        }

        return AzureResource;
    }

    /// <summary>
    /// Gets the client.
    /// </summary>
    /// <returns>ArmClient.</returns>
    public override ArmClient GetClient()
    {
        return ResourceGroup.GetClient();
    }
}