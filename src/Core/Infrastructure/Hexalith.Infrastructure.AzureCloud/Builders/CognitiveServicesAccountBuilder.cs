// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.AzureCloud
// Author           : Jérôme Piquot
// Created          : 04-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-06-2023
// ***********************************************************************
// <copyright file="CognitiveServicesAccountBuilder.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.AzureCloud.Builders;

using System.Threading;
using System.Threading.Tasks;

using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.CognitiveServices;
using Azure.ResourceManager.Resources;

/// <summary>
/// Class ResourceGroupBuilder.
/// </summary>
public class CognitiveServicesAccountBuilder : ResourceGroupItemBuilder<CognitiveServicesAccountResource, CognitiveServicesAccountData>
{
    private const string TypeName = "Cognitive Services Account";

    /// <summary>
    /// Initializes a new instance of the <see cref="CognitiveServicesAccountBuilder"/> class.
    /// </summary>
    /// <param name="azureBuilder">The azure builder.</param>
    /// <param name="resourceGroupBuilder">The resource group builder.</param>
    /// <param name="name">The name.</param>
    /// <param name="kind">The kind.</param>
    /// <param name="location">The location.</param>
    /// <param name="data">The data.</param>
    public CognitiveServicesAccountBuilder(
        AzureBuilder azureBuilder,
        ResourceGroupBuilder resourceGroupBuilder,
        string name,
        string kind,
        string? location,
        CognitiveServicesAccountData? data)
        : base(azureBuilder, resourceGroupBuilder, TypeName, name, data, false)
    {
        ResourceGroup = resourceGroupBuilder;
        Name = name;
        Kind = kind;
        Location = location;
        Data = data;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CognitiveServicesAccountBuilder"/> class.
    /// </summary>
    /// <param name="azureBuilder">The azure builder.</param>
    /// <param name="resourceGroupBuilder">The resource group builder.</param>
    /// <param name="name">The name.</param>
    /// <param name="existing">if set to <c>true</c> [existing].</param>
    public CognitiveServicesAccountBuilder(
        AzureBuilder azureBuilder,
        ResourceGroupBuilder resourceGroupBuilder,
        string name,
        bool existing = false)
        : base(azureBuilder, resourceGroupBuilder, TypeName, name, null, existing)
    {
        ResourceGroup = resourceGroupBuilder;
        Name = name;
        Kind = string.Empty;
    }

    /// <summary>
    /// Gets the data.
    /// </summary>
    /// <value>The data.</value>
    public new CognitiveServicesAccountData? Data { get; private set; }

    /// <summary>
    /// Gets the kind.
    /// </summary>
    /// <value>The kind.</value>
    public string Kind { get; }

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
    /// Gets the resourceGroup.
    /// </summary>
    /// <value>The resourceGroup.</value>
    public new ResourceGroupBuilder ResourceGroup { get; }

    /// <inheritdoc/>
    protected override async Task<CognitiveServicesAccountResource> CreateOrUpdateResourceAsync(CancellationToken cancellationToken)
    {
        ResourceGroupResource group = await ResourceGroup.BuildAsync(cancellationToken).ConfigureAwait(false);
        CognitiveServicesAccountCollection accounts = group.GetCognitiveServicesAccounts();

        if (Data == null)
        {
            AzureLocation location = string.IsNullOrWhiteSpace(Location) ? group.Data.Location : Location;
            Data = new(location)
            {
                Kind = Kind,
                Sku = new Azure.ResourceManager.CognitiveServices.Models.CognitiveServicesSku("S0"),
            };
        }

        ArmOperation<CognitiveServicesAccountResource> operation = await accounts
            .CreateOrUpdateAsync(
                Azure.WaitUntil.Completed,
                Name,
                Data,
                cancellationToken).ConfigureAwait(false);
        return operation.Value;
    }

    /// <inheritdoc/>
    protected override async Task<bool> ExistsAsync(CancellationToken cancellationToken)
    {
        ResourceGroupResource group = await ResourceGroup.BuildAsync(cancellationToken).ConfigureAwait(false);
        return (await group.GetCognitiveServicesAccounts().ExistsAsync(Name, cancellationToken).ConfigureAwait(false)).Value;
    }

    /// <inheritdoc/>
    protected override async Task<CognitiveServicesAccountResource> GetExistingResourceAsync(CancellationToken cancellationToken)
    {
        ResourceGroupResource group = await ResourceGroup.BuildAsync(cancellationToken).ConfigureAwait(false);
        return (await group.GetCognitiveServicesAccounts().GetAsync(Name, cancellationToken).ConfigureAwait(false)).Value;
    }
}