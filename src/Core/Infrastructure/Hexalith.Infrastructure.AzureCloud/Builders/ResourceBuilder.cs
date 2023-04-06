// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.AzureCloud
// Author           : Jérôme Piquot
// Created          : 04-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-04-2023
// ***********************************************************************
// <copyright file="ResourceBuilder.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.AzureCloud.Builders;

using Azure.ResourceManager;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class ResourceBuilder.
/// </summary>
/// <typeparam name="TArmResource">The type of the t arm resource.</typeparam>
public abstract class ResourceBuilder<TArmResource> : IResourceBuilder
    where TArmResource : ArmResource
{
    private ILogger? _logger;
    private string? _resourceBuilderId;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceBuilder{TArmResource}"/> class.
    /// </summary>
    /// <param name="azureBuilder">The azure builder.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="uniqueId">The unique identifier.</param>
    /// <param name="existing">if set to <c>true</c> [existing].</param>
    public ResourceBuilder(AzureBuilder azureBuilder, IResourceBuilder? parent, string uniqueId, bool existing = false)
    {
        AzureBuilder = azureBuilder;
        Parent = parent;
        Existing = existing;
        UniqueId = uniqueId;
        AzureBuilder.AddResource(this);
    }

    /// <summary>
    /// Gets the azure builder.
    /// </summary>
    /// <value>The azure builder.</value>
    public AzureBuilder AzureBuilder { get; }

    /// <summary>
    /// Gets a value indicating whether this <see cref="ResourceBuilder{TArmResource}" /> is existing.
    /// </summary>
    /// <value><c>true</c> if existing; otherwise, <c>false</c>.</value>
    public bool Existing { get; }

    /// <summary>
    /// Gets the azure builder.
    /// </summary>
    /// <value>The azure builder.</value>
    public IResourceBuilder? Parent { get; }

    /// <inheritdoc/>
    public string ResourceBuilderExistingId => _resourceBuilderId ??= $"{GetType().Name}-{UniqueId}-Existing";

    /// <inheritdoc/>
    public string ResourceBuilderId => Existing ? ResourceBuilderExistingId : ResourceBuilderNotExistingId;

    /// <inheritdoc/>
    public string ResourceBuilderNotExistingId => _resourceBuilderId ??= $"{GetType().Name}-{UniqueId}";

    /// <summary>
    /// Gets the unique identifier.
    /// </summary>
    /// <value>The unique identifier.</value>
    public string UniqueId { get; }

    /// <summary>
    /// Gets the logger.
    /// </summary>
    /// <value>The logger.</value>
    protected ILogger Logger => _logger ??= AzureBuilder.LoggerFactory.CreateLogger(GetType());

    /// <summary>
    /// Gets or sets the resource.
    /// </summary>
    /// <value>The resource.</value>
    protected TArmResource? Resource { get; set; }

    /// <summary>
    /// Builds the asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;TArmResource&gt;&gt;.</returns>
    public abstract Task<TArmResource> BuildAsync(CancellationToken cancellationToken);

    /// <inheritdoc/>
    async Task<ArmResource> IResourceBuilder.BuildAsync(CancellationToken cancellationToken)
    {
        return await BuildAsync(cancellationToken);
    }
}