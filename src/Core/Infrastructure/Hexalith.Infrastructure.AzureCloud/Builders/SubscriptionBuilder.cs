// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.AzureCloud
// Author           : Jérôme Piquot
// Created          : 04-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-07-2023
// ***********************************************************************
// <copyright file="SubscriptionBuilder.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.AzureCloud.Builders;

using System.Threading;
using System.Threading.Tasks;

using Azure.ResourceManager.Resources;

/// <summary>
/// Class SubscriptionBuilder.
/// </summary>
public class SubscriptionBuilder : ResourceBuilder<SubscriptionResource>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SubscriptionBuilder" /> class.
    /// </summary>
    /// <param name="azureBuilder">The azure builder.</param>
    /// <param name="id">The identifier.</param>
    public SubscriptionBuilder(AzureBuilder azureBuilder, string id)
        : base(azureBuilder, null, "Subscription", id, true) => Id = id;

    /// <summary>
    /// Gets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public string? Id { get; }

    /// <inheritdoc/>
    public override async Task<SubscriptionResource> BuildAsync(CancellationToken cancellationToken) => Resource ??= await GetExistingResourceAsync(cancellationToken).ConfigureAwait(false);

    /// <inheritdoc/>
    protected override Task<SubscriptionResource> CreateOrUpdateResourceAsync(CancellationToken cancellationToken) => throw new NotSupportedException("Can't create an Azure Subscription.");

    /// <inheritdoc/>
    protected override async Task<bool> ExistsAsync(CancellationToken cancellationToken)
    {
        Azure.Response<bool> response = await AzureBuilder
            .Client
            .GetSubscriptions()
            .ExistsAsync(Id, cancellationToken)
            .ConfigureAwait(false);
        return response.Value;
    }

    /// <inheritdoc/>
    protected override async Task<SubscriptionResource> GetExistingResourceAsync(CancellationToken cancellationToken)
    {
        SubscriptionResource subscription = AzureBuilder
            .Client
            .GetSubscriptionResource(SubscriptionResource.CreateResourceIdentifier(Id));
        return await subscription
            .GetAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}