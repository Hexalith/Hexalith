// ***********************************************************************
// Assembly         : Deploy
// Author           : Jérôme Piquot
// Created          : 04-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-03-2023
// ***********************************************************************
// <copyright file="Global.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Deploy.Infrastructure.Global;

using Azure.Identity;
using Azure.ResourceManager;

using Deploy.Infrastructure.Resources;

/// <summary>
/// Class Global.
/// </summary>
internal class Global
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Global" /> class.
    /// </summary>
    /// <param name="subscriptionId">The subscription identifier.</param>
    /// <param name="resourceGroupName">Name of the resource group.</param>
    /// <param name="containerRegistryName">Name of the container registry.</param>
    /// <param name="location">The location.</param>
    /// <exception cref="System.ArgumentNullException">Null.</exception>
    public Global(
        string subscriptionId,
        string resourceGroupName,
        string containerRegistryName,
        string location)
    {
        ArgumentException.ThrowIfNullOrEmpty(subscriptionId);
        ArgumentException.ThrowIfNullOrEmpty(containerRegistryName);
        ArgumentException.ThrowIfNullOrEmpty(resourceGroupName);
        ArgumentException.ThrowIfNullOrEmpty(location);
        SubscriptionId = subscriptionId;
        ContainerRegistryName = containerRegistryName;
        ResourceGroupName = resourceGroupName;
        Location = location;
    }

    /// <summary>
    /// Gets the name of the container registry.
    /// </summary>
    /// <value>The name of the container registry.</value>
    public string ContainerRegistryName { get; }

    /// <summary>
    /// Gets the location.
    /// </summary>
    /// <value>The location.</value>
    public string Location { get; }

    /// <summary>
    /// Gets the name of the resource group.
    /// </summary>
    /// <value>The name of the resource group.</value>
    public string ResourceGroupName { get; }

    /// <summary>
    /// Gets the subscription identifier.
    /// </summary>
    /// <value>The subscription identifier.</value>
    public string SubscriptionId { get; }

    /// <summary>
    /// Deploys the asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A <see cref="Task" /> representing the result of the asynchronous operation.</returns>
    public async Task DeployAsync(CancellationToken cancellationToken)
    {
        Subscription subscription = new(GetArmClient(SubscriptionId), SubscriptionId);
        ResourceGroup resourceGroup = new(subscription, ResourceGroupName, Location);
        _ = await resourceGroup.CreateOrUpdateAsync(cancellationToken);
    }

    /// <summary>
    /// Initializes the arm client.
    /// </summary>
    /// <param name="subscriptionId">The subscription identifier.</param>
    /// <returns>System.Nullable&lt;ArmClient&gt;.</returns>
    private static ArmClient GetArmClient(string? subscriptionId)
    {
        return new ArmClient(new DefaultAzureCredential(), subscriptionId);
    }
}