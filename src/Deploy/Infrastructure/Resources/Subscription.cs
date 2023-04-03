// ***********************************************************************
// Assembly         : Deploy
// Author           : Jérôme Piquot
// Created          : 04-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-03-2023
// ***********************************************************************
// <copyright file="Subscription.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Deploy.Infrastructure.Resources;

using System.Threading.Tasks;

using Azure.ResourceManager;
using Azure.ResourceManager.Resources;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class Subscription.
/// Implements the <see cref="Deploy.Infrastructure.Resources.Resource{Azure.ResourceManager.Resources.SubscriptionResource}" />
/// Implements the <see cref="System.IEquatable{Deploy.Infrastructure.Resources.Resource{Azure.ResourceManager.Resources.SubscriptionResource}}" />
/// Implements the <see cref="System.IEquatable{Deploy.Infrastructure.Resources.Subscription}" />.
/// </summary>
/// <seealso cref="Deploy.Infrastructure.Resources.Resource{Azure.ResourceManager.Resources.SubscriptionResource}" />
/// <seealso cref="System.IEquatable{Deploy.Infrastructure.Resources.Resource{Azure.ResourceManager.Resources.SubscriptionResource}}" />
/// <seealso cref="System.IEquatable{Deploy.Infrastructure.Resources.Subscription}" />
internal record Subscription(ArmClient ArmClient, string Id, ILogger Logger) : Resource<SubscriptionResource>(Logger)
{
    /// <summary>
    /// Creates the or update asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    /// <exception cref="System.NotSupportedException">Subscriptions can not be created by ARM.</exception>
    public override Task<SubscriptionResource> CreateOrUpdateAsync(CancellationToken cancellationToken)
    {
        throw new NotSupportedException("Subscriptions can not be created by ARM.");
    }

    /// <summary>
    /// Gets the resource asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;SubscriptionResource&gt;.</returns>
    public override Task<SubscriptionResource> GetResourceAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(AzureResource ??= ArmClient.GetSubscriptionResource(SubscriptionResource.CreateResourceIdentifier(Id)));
    }

    /// <summary>
    /// Gets the client.
    /// </summary>
    /// <returns>ArmClient.</returns>
    public override ArmClient GetClient()
    {
        return ArmClient;
    }
}