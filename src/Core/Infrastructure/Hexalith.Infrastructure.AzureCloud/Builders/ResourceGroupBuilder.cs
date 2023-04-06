// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.AzureCloud
// Author           : Jérôme Piquot
// Created          : 04-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-06-2023
// ***********************************************************************
// <copyright file="ResourceGroupBuilder.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.AzureCloud.Builders;

using System.Threading;
using System.Threading.Tasks;

using Azure.ResourceManager;
using Azure.ResourceManager.Resources;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class ResourceGroupBuilder.
/// </summary>
public class ResourceGroupBuilder : ResourceBuilder<ResourceGroupResource>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceGroupBuilder" /> class.
    /// </summary>
    /// <param name="azureBuilder">The azure builder.</param>
    /// <param name="subscriptionBuilder">The subscription builder.</param>
    /// <param name="name">The name.</param>
    /// <param name="location">The location.</param>
    public ResourceGroupBuilder(AzureBuilder azureBuilder, SubscriptionBuilder subscriptionBuilder, string name, string location)
        : base(azureBuilder, subscriptionBuilder, name, false)
    {
        Subscription = subscriptionBuilder;
        Name = name;
        Location = location;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceGroupBuilder" /> class.
    /// </summary>
    /// <param name="azureBuilder">The azure builder.</param>
    /// <param name="subscriptionBuilder">The subscription builder.</param>
    /// <param name="name">The name.</param>
    /// <param name="existing">if set to <c>true</c> [existing].</param>
    public ResourceGroupBuilder(AzureBuilder azureBuilder, SubscriptionBuilder subscriptionBuilder, string name, bool existing = false)
        : base(azureBuilder, subscriptionBuilder, name, existing)
    {
        Subscription = subscriptionBuilder;
        Name = name;
        Location = string.Empty;
    }

    /// <summary>
    /// Gets the location.
    /// </summary>
    /// <value>The location.</value>
    public string Location { get; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>The name.</value>
    public string Name { get; }

    /// <summary>
    /// Gets the subscription.
    /// </summary>
    /// <value>The subscription.</value>
    public SubscriptionBuilder Subscription { get; }

    /// <summary>
    /// Adds the container registry.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="sku">The sku.</param>
    /// <param name="location">The location.</param>
    /// <returns>ContainerRegistryBuilder.</returns>
    public ContainerRegistryBuilder AddContainerRegistry(string name, string? sku, string? location)
    {
        ContainerRegistryBuilder builder = new(
            AzureBuilder,
            this,
            name,
            sku,
            location ?? Location,
            null,
            false);
        return builder;
    }

    /// <inheritdoc/>
    public override async Task<ResourceGroupResource> BuildAsync(CancellationToken cancellationToken)
    {
        if (Resource == null)
        {
            SubscriptionResource subscriptionResource = await Subscription.BuildAsync(cancellationToken);
            SubscriptionData subscriptionData = subscriptionResource.Data;
            ResourceGroupCollection resourceGroups = subscriptionResource.GetResourceGroups();
            ResourceGroupData resourceGroupData = new(Location);
            Logger.LogInformation("Creating resource group {Name} in subscription {Subscription}/{SubscriptionId} in location {Location}", Name, subscriptionData.DisplayName, subscriptionData.SubscriptionId, Location);
            ArmOperation<ResourceGroupResource> operation = await resourceGroups
                .CreateOrUpdateAsync(
                    Azure.WaitUntil.Completed,
                    Name,
                    resourceGroupData,
                    cancellationToken);
            Logger.LogInformation("Resource group {Name} created", Name);
            Resource = operation.Value;
        }

        return Resource;
    }
}