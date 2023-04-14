// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.AzureCloud
// Author           : Jérôme Piquot
// Created          : 04-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-06-2023
// ***********************************************************************
// <copyright file="KeyVaultBuilder.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.AzureCloud.Builders;

using System;
using System.Threading;
using System.Threading.Tasks;

using Azure.ResourceManager.KeyVault;
using Azure.ResourceManager.KeyVault.Models;
using Azure.ResourceManager.Resources;

/// <summary>
/// Class KeyVaultBuilder.
/// Implements the <see cref="Hexalith.Infrastructure.AzureCloud.Builders.ResourceBuilder{Azure.ResourceManager.KeyVault.KeyVaultResource}" />.
/// </summary>
/// <seealso cref="Hexalith.Infrastructure.AzureCloud.Builders.ResourceBuilder{Azure.ResourceManager.KeyVault.KeyVaultResource}" />
public class KeyVaultBuilder : ResourceGroupItemBuilder<KeyVaultResource, KeyVaultData>
{
    private const string TypeName = "Key Vault";

    private readonly Dictionary<string, string> _secrets = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyVaultBuilder" /> class.
    /// </summary>
    /// <param name="azureBuilder">The azure builder.</param>
    /// <param name="resourceGroup">The resource group.</param>
    /// <param name="name">The name.</param>
    /// <param name="sku">The sku.</param>
    /// <param name="location">The location.</param>
    /// <param name="data">The data.</param>
    /// <param name="existing">if set to <c>true</c> [existing].</param>
    public KeyVaultBuilder(
        AzureBuilder azureBuilder,
        ResourceGroupBuilder resourceGroup,
        string name,
        KeyVaultSkuName? sku,
        string? location,
        KeyVaultData? data)
        : base(azureBuilder, resourceGroup, TypeName, name, data, false)
    {
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
    /// Initializes a new instance of the <see cref="KeyVaultBuilder"/> class.
    /// </summary>
    /// <param name="azureBuilder">The azure builder.</param>
    /// <param name="resourceGroup">The resource group.</param>
    /// <param name="name">The name.</param>
    /// <param name="existing">if set to <c>true</c> [existing].</param>
    public KeyVaultBuilder(
        AzureBuilder azureBuilder,
        ResourceGroupBuilder resourceGroup,
        string name,
        bool existing)
        : base(azureBuilder, resourceGroup, TypeName, name, null, existing)
    {
        Name = name;
    }

    /// <summary>
    /// Gets the data.
    /// </summary>
    /// <value>The data.</value>
    public new KeyVaultData? Data { get; private set; }

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
    public KeyVaultSkuName? Sku { get; }

    /// <summary>
    /// Adds the secret.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="value">The value.</param>
    /// <returns>KeyVaultBuilder.</returns>
    public KeyVaultBuilder AddSecret(string name, string value)
    {
        if (Existing == true)
        {
            throw new InvalidOperationException("Can't add secret to an existing Key Vault.");
        }

        _secrets.Add(name, value);
        return this;
    }

    /// <inheritdoc/>
    protected override async Task<KeyVaultResource> CreateOrUpdateResourceAsync(CancellationToken cancellationToken)
    {
        ResourceGroupResource group = await ResourceGroup.BuildAsync(cancellationToken).ConfigureAwait(false);
        KeyVaultCollection keyVaults = group.GetKeyVaults();
        if (await keyVaults.ExistsAsync(Name))
        {
            Azure.Response<KeyVaultResource> keyVaultResponse = await group.GetKeyVaultAsync(Name, cancellationToken);
            return keyVaultResponse.Value;
        }

        if (Data == null)
        {
            SubscriptionResource subscription = await ResourceGroup.Subscription.BuildAsync(cancellationToken);
            Data = new KeyVaultData(
                Location ?? group.Data.Location,
                new KeyVaultProperties(
                    subscription.Data.TenantId ?? throw new InvalidOperationException($"Subscription {subscription.Data.DisplayName}/{subscription.Data.SubscriptionId} is not attached to an Azure Active Directory tenant."),
                    new KeyVaultSku(KeyVaultSkuFamily.A, Sku ?? KeyVaultSkuName.Standard)));
        }

        Azure.ResourceManager.ArmOperation<KeyVaultResource> operation = await keyVaults
            .CreateOrUpdateAsync(
                Azure.WaitUntil.Completed,
                Name,
                new KeyVaultCreateOrUpdateContent(
                    Data.Location,
                    Data.Properties),
                cancellationToken);
        KeyVaultResource keyVault = operation.Value;
        /*
        bool createPolicy = true;
        foreach (var policy in Data.Properties.AccessPolicies)
        {
            if (policy.ObjectId == keyVault.Data.Properties.TenantId)
            {
                createPolicy = false;
                break;
            }
        }
        SecretClient secretsClient = new(keyVault.Data.Properties.VaultUri, new DefaultAzureCredential());
        foreach (KeyValuePair<string, string> secret in _secrets)
        {
            if (await secretsClient.GetSecretAsync(secret.Key, null, cancellationToken) == null)
            {
                _ = await secretsClient.SetSecretAsync(secret.Key, secret.Value, cancellationToken);
            }
        }
        */
        return keyVault;
    }

    /// <inheritdoc/>
    protected override async Task<bool> ExistsAsync(CancellationToken cancellationToken)
    {
        ResourceGroupResource resource = await ResourceGroup.BuildAsync(cancellationToken).ConfigureAwait(false);
        return await resource.GetKeyVaults().ExistsAsync(Name, cancellationToken);
    }

    /// <inheritdoc/>
    protected override async Task<KeyVaultResource> GetExistingResourceAsync(CancellationToken cancellationToken)
    {
        ResourceGroupResource resource = await ResourceGroup.BuildAsync(cancellationToken).ConfigureAwait(false);
        return await resource.GetKeyVaults().GetAsync(Name, cancellationToken);
    }
}