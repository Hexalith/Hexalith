// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.AzureCloud
// Author           : Jérôme Piquot
// Created          : 04-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-06-2023
// ***********************************************************************
// <copyright file="ContainerRegistryBuilder.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.AzureCloud.Builders;

using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.ContainerRegistry;
using Azure.ResourceManager.ContainerRegistry.Models;
using Azure.ResourceManager.Resources;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class ContainerRegistryBuilder.
/// Implements the <see cref="Hexalith.Infrastructure.AzureCloud.Builders.ResourceBuilder{Azure.ResourceManager.ContainerRegistry.ContainerRegistryResource}" />.
/// </summary>
/// <seealso cref="Hexalith.Infrastructure.AzureCloud.Builders.ResourceBuilder{Azure.ResourceManager.ContainerRegistry.ContainerRegistryResource}" />
public class ContainerRegistryBuilder : ResourceGroupItemBuilder<ContainerRegistryResource, ContainerRegistryData>
{
    private const string TypeName = "Container Registry";

    /// <summary>
    /// The resource group.
    /// </summary>
    private readonly ResourceGroupBuilder _resourceGroup;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContainerRegistryBuilder" /> class.
    /// </summary>
    /// <param name="azureBuilder">The azure builder.</param>
    /// <param name="resourceGroup">The resource group.</param>
    /// <param name="name">The name.</param>
    /// <param name="sku">The sku.</param>
    /// <param name="location">The location.</param>
    /// <param name="data">The data.</param>
    /// <param name="existing">if set to <c>true</c> [existing].</param>
    public ContainerRegistryBuilder(
        AzureBuilder azureBuilder,
        ResourceGroupBuilder resourceGroup,
        string name,
        string? sku,
        string? location,
        ContainerRegistryData? data,
        bool existing)
        : base(azureBuilder, resourceGroup, TypeName, name, data, existing)
    {
        _resourceGroup = resourceGroup;
        Name = name;
        Sku = sku;
        Location = location;
        Data = data;
        if (data != null && (sku != null || location != null))
        {
            throw new ArgumentException("Data can't be used if sku or location have values.", nameof(data));
        }
    }

    /// <summary>
    /// Gets the data.
    /// </summary>
    /// <value>The data.</value>
    public new ContainerRegistryData? Data { get; private set; }

    /// <summary>
    /// Gets the location.
    /// </summary>
    /// <value>The location.</value>
    public string? Location { get; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>The name.</value>
    public string Name { get; }

    /// <summary>
    /// Gets the sku.
    /// </summary>
    /// <value>The sku.</value>
    public string? Sku { get; }

    /// <inheritdoc/>
    protected override async Task<ContainerRegistryResource> CreateOrUpdateResourceAsync(CancellationToken cancellationToken)
    {
        ResourceGroupResource group = await _resourceGroup.BuildAsync(cancellationToken);
        ContainerRegistryCollection registries = group.GetContainerRegistries();
        if (Data == null)
        {
            ContainerRegistrySkuName sku = string.IsNullOrWhiteSpace(Sku) ? ContainerRegistrySkuName.Basic : new ContainerRegistrySkuName(Sku);
            AzureLocation location = string.IsNullOrWhiteSpace(Location) ? group.Data.Location : Location;
            Data = new(location, new ContainerRegistrySku(sku));
        }

        Logger.LogInformation("Creating container registry {Name} in resource group {ResourceGroup} in location {Location} with sku {Sku}", Name, Name, Location, Sku);
        ArmOperation<ContainerRegistryResource> operation = await registries
            .CreateOrUpdateAsync(
                Azure.WaitUntil.Completed,
                Name,
                Data,
                cancellationToken);
        Logger.LogInformation("Container registry {Name} created", Name);
        return operation.Value;
    }

    /// <inheritdoc/>
    protected override async Task<bool> ExistsAsync(CancellationToken cancellationToken)
    {
        ResourceGroupResource group = await _resourceGroup.BuildAsync(cancellationToken);
        return (await group.GetContainerRegistries().ExistsAsync(Name, cancellationToken)).Value;
    }

    /// <inheritdoc/>
    protected override async Task<ContainerRegistryResource> GetExistingResourceAsync(CancellationToken cancellationToken)
    {
        ResourceGroupResource group = await _resourceGroup.BuildAsync(cancellationToken);
        return (await group.GetContainerRegistries().GetAsync(Name, cancellationToken)).Value;
    }
}