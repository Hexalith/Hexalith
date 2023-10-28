// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.AzureCloud
// Author           : Jérôme Piquot
// Created          : 04-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-07-2023
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
using Azure.ResourceManager.KeyVault.Models;
using Azure.ResourceManager.Resources;

/// <summary>
/// Class ResourceGroupBuilder.
/// </summary>
public class ResourceGroupBuilder : ResourceBuilder<ResourceGroupResource>
{
    /// <summary>
    /// The type name.
    /// </summary>
    private const string TypeName = "Resource Group";

    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceGroupBuilder" /> class.
    /// </summary>
    /// <param name="azureBuilder">The azure builder.</param>
    /// <param name="subscriptionBuilder">The subscription builder.</param>
    /// <param name="name">The name.</param>
    /// <param name="location">The location.</param>
    public ResourceGroupBuilder(AzureBuilder azureBuilder, SubscriptionBuilder subscriptionBuilder, string name, string location)
        : base(azureBuilder, subscriptionBuilder, TypeName, name, false)
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
        : base(azureBuilder, subscriptionBuilder, TypeName, name, existing)
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

    /// <summary>
    /// Adds the key vault.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="sku">The sku.</param>
    /// <param name="location">The location.</param>
    /// <returns>KeyVaultBuilder.</returns>
    public KeyVaultBuilder AddKeyVault(string name, KeyVaultSkuName? sku, string? location) => new(AzureBuilder, this, name, sku, location, null);

    /// <summary>
    /// Adds the logic workflow.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="location">The location.</param>
    /// <returns>LogicWorkflowBuilder.</returns>
    public LogicWorkflowBuilder AddLogicWorkflow(string name, string? location) => new(AzureBuilder, this, name, location, null);

    /// <summary>
    /// Adds the cognitive services account.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="location">The location.</param>
    /// <returns>System.Object.</returns>
    public CognitiveServicesAccountBuilder AddOpenAIAccount(string name, string location) => new(AzureBuilder, this, name, "OpenAI", location, null);

    /// <inheritdoc/>
    public override async Task<ResourceGroupResource> BuildAsync(CancellationToken cancellationToken)
    {
        Resource ??= Existing ? await GetExistingResourceAsync(cancellationToken).ConfigureAwait(false) : await CreateOrUpdateResourceAsync(cancellationToken).ConfigureAwait(false);

        return Resource;
    }

    /// <inheritdoc/>
    protected override async Task<ResourceGroupResource> CreateOrUpdateResourceAsync(CancellationToken cancellationToken)
    {
        SubscriptionResource subscriptionResource = await Subscription.BuildAsync(cancellationToken).ConfigureAwait(false);
        ResourceGroupCollection resourceGroups = subscriptionResource.GetResourceGroups();
        ResourceGroupData resourceGroupData = new(Location);
        ArmOperation<ResourceGroupResource> operation = await resourceGroups
            .CreateOrUpdateAsync(
                Azure.WaitUntil.Completed,
                Name,
                resourceGroupData,
                cancellationToken).ConfigureAwait(false);
        return operation.Value;
    }

    /// <inheritdoc/>
    protected override async Task<bool> ExistsAsync(CancellationToken cancellationToken)
    {
        SubscriptionResource subscription = await Subscription.BuildAsync(cancellationToken).ConfigureAwait(false);
        return (await subscription.GetResourceGroups().ExistsAsync(Name, cancellationToken).ConfigureAwait(false)).Value;
    }

    /// <inheritdoc/>
    protected override async Task<ResourceGroupResource> GetExistingResourceAsync(CancellationToken cancellationToken)
    {
        SubscriptionResource subscription = await Subscription.BuildAsync(cancellationToken).ConfigureAwait(false);
        return (await subscription.GetResourceGroups().GetAsync(Name, cancellationToken).ConfigureAwait(false)).Value;
    }
}