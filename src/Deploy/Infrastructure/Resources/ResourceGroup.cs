// ***********************************************************************
// Assembly         : Deploy
// Author           : Jérôme Piquot
// Created          : 04-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-02-2023
// ***********************************************************************
// <copyright file="ResourceGroup.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Deploy.Infrastructure.Resources;

using Azure.ResourceManager;
using Azure.ResourceManager.Resources;

/// <summary>
/// Class ResourceGroup.
/// Implements the <see cref="Deploy.Infrastructure.Resources.Resource{Azure.ResourceManager.Resources.ResourceGroupResource}" />
/// Implements the <see cref="System.IEquatable{Deploy.Infrastructure.Resources.Resource{Azure.ResourceManager.Resources.ResourceGroupResource}}" />
/// Implements the <see cref="System.IEquatable{Deploy.Infrastructure.Resources.ResourceGroup}" />.
/// </summary>
/// <seealso cref="Deploy.Infrastructure.Resources.Resource{Azure.ResourceManager.Resources.ResourceGroupResource}" />
/// <seealso cref="System.IEquatable{Deploy.Infrastructure.Resources.Resource{Azure.ResourceManager.Resources.ResourceGroupResource}}" />
/// <seealso cref="System.IEquatable{Deploy.Infrastructure.Resources.ResourceGroup}" />
internal record ResourceGroup(Subscription Subscription, string Name, string Location) : Resource<ResourceGroupResource>
{
    /// <inheritdoc/>
    public override async Task<ResourceGroupResource> CreateOrUpdateAsync(CancellationToken cancellationToken)
    {
        SubscriptionResource subscriptionResource = await Subscription.GetResourceAsync(cancellationToken);
        ResourceGroupCollection resourceGroups = subscriptionResource.GetResourceGroups();
        ResourceGroupData resourceGroupData = new(Location);

        ArmOperation<ResourceGroupResource> operation = await resourceGroups
            .CreateOrUpdateAsync(
                Azure.WaitUntil.Completed,
                Name,
                resourceGroupData,
                cancellationToken);
        return operation.Value;
    }

    /// <inheritdoc/>
    public override async Task<ResourceGroupResource> GetResourceAsync(CancellationToken cancellationToken)
    {
        if (AzureResource == null)
        {
            SubscriptionResource subscription = await Subscription.GetResourceAsync(cancellationToken);
            AzureResource = await subscription.GetResourceGroupAsync(Name, cancellationToken);
        }

        return AzureResource;
    }

    /// <summary>
    /// Gets the client.
    /// </summary>
    /// <returns>ArmClient.</returns>
    public override ArmClient GetClient()
    {
        return Subscription.GetClient();
    }
}