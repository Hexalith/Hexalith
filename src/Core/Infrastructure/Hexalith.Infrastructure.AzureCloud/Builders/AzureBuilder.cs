// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.AzureCloud
// Author           : Jérôme Piquot
// Created          : 04-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-06-2023
// ***********************************************************************
// <copyright file="AzureBuilder.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.AzureCloud.Builders;

using System.Collections.Generic;

using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;

using Hexalith.Application.TopologicalSorting;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class AzureBuilder.
/// </summary>
public class AzureBuilder
{
    /// <summary>
    /// The ordered processes.
    /// </summary>
    private readonly Dictionary<string, OrderedProcess> _orderedProcesses = new();

    /// <summary>
    /// The resources.
    /// </summary>
    private readonly Dictionary<string, IResourceBuilder> _resources = new();

    /// <summary>
    /// The client.
    /// </summary>
    private ArmClient? _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="AzureBuilder" /> class.
    /// </summary>
    /// <param name="tokenCredential">The token credential.</param>
    /// <param name="subscriptionId">The subscription identifier.</param>
    /// <param name="loggerFactory">The logger factory.</param>
    public AzureBuilder(TokenCredential tokenCredential, string subscriptionId, ILoggerFactory loggerFactory)
    {
        TokenCredential = tokenCredential;
        LoggerFactory = loggerFactory;
        Subscription = new SubscriptionBuilder(this, subscriptionId);
        DependencyGraph = new DependencyGraph();
        Logger = loggerFactory.CreateLogger<AzureBuilder>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AzureBuilder" /> class.
    /// </summary>
    /// <param name="subscriptionId">The subscription identifier.</param>
    /// <param name="loggerFactory">The logger factory.</param>
    public AzureBuilder(string subscriptionId, ILoggerFactory loggerFactory)
        : this(new DefaultAzureCredential(), subscriptionId, loggerFactory)
    {
    }

    /// <summary>
    /// Gets the client.
    /// </summary>
    /// <value>The client.</value>
    public ArmClient Client => _client ??= new ArmClient(TokenCredential);

    /// <summary>
    /// Gets the dependency graph.
    /// </summary>
    /// <value>The dependency graph.</value>
    public DependencyGraph DependencyGraph { get; }

    /// <summary>
    /// Gets the logger factory.
    /// </summary>
    /// <value>The logger factory.</value>
    public ILoggerFactory LoggerFactory { get; }

    /// <summary>
    /// Gets the ordered processes.
    /// </summary>
    /// <value>The ordered processes.</value>
    public IReadOnlyDictionary<string, OrderedProcess> OrderedProcesses => _orderedProcesses;

    /// <summary>
    /// Gets the subscription.
    /// </summary>
    /// <value>The subscription.</value>
    public IReadOnlyDictionary<string, IResourceBuilder> Resources => _resources;

    /// <summary>
    /// Gets the subscription.
    /// </summary>
    /// <value>The subscription.</value>
    public SubscriptionBuilder Subscription { get; }

    /// <summary>
    /// Gets the token credential.
    /// </summary>
    /// <value>The token credential.</value>
    public TokenCredential TokenCredential { get; }

    /// <summary>
    /// Gets the logger.
    /// </summary>
    /// <value>The logger.</value>
    private ILogger<AzureBuilder> Logger { get; }

    /// <summary>
    /// Adds the resource.
    /// </summary>
    /// <param name="resourceBuilder">The resource builder.</param>
    public void AddResource(IResourceBuilder resourceBuilder)
    {
        _ = new OrderedProcess(DependencyGraph, resourceBuilder.ResourceBuilderNotExistingId);
        OrderedProcess notExisting = _orderedProcesses[resourceBuilder.ResourceBuilderNotExistingId];
        _ = _resources.TryAdd(resourceBuilder.ResourceBuilderId, resourceBuilder);
        OrderedProcess child = notExisting;
        if (resourceBuilder.Existing)
        {
            // If the resource should already exists, we add a dependency to the resource that should be created or updated before.
            OrderedProcess existing = new(DependencyGraph, resourceBuilder.ResourceBuilderExistingId);
            _ = _orderedProcesses.TryAdd(resourceBuilder.ResourceBuilderExistingId, existing);
            existing = _orderedProcesses[resourceBuilder.ResourceBuilderExistingId];
            _ = notExisting.Before(existing);
            child = existing;
        }

        if (resourceBuilder.Parent != null)
        {
            OrderedProcess parent = _orderedProcesses[resourceBuilder.Parent.ResourceBuilderId];
            _ = parent.Before(child);
        }
    }

    /// <summary>
    /// Adds the resource group.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="location">The location.</param>
    /// <returns>ResourceGroupBuilder.</returns>
    public ResourceGroupBuilder AddResourceGroup(string name, string location)
    {
        ResourceGroupBuilder builder = new(this, Subscription, name, location);
        return builder;
    }

    /// <summary>
    /// Build as an asynchronous operation.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task BuildAsync(CancellationToken cancellationToken)
    {
        DateTimeOffset startDateTime = DateTimeOffset.Now;
        Logger.LogInformation("Azure Cloud deployment started at : {DeploymentStartDateTime}.", startDateTime);
        ArmClient armClient = new(TokenCredential ?? new DefaultAzureCredential());

        IEnumerable<ISet<OrderedProcess>> sort = DependencyGraph.CalculateSort();
        int step = 0;
        foreach (ISet<OrderedProcess> bucket in sort)
        {
            HashSet<string> taskNames = bucket.Select(p => p.Name).ToHashSet();
            List<KeyValuePair<string, IResourceBuilder>> tasks = Resources
                        .Where(p => taskNames.Contains(p.Key))
                        .ToList();
            if (tasks.Any())
            {
                Logger.LogInformation("Step {Step}: Handling resources: {ResourceNames}.", ++step, string.Join(", ", taskNames));
                _ = await Task.WhenAll(tasks.Select(p => p.Value.BuildAsync(cancellationToken)));
            }
        }

        DateTimeOffset endDateTime = DateTimeOffset.Now;
        Logger.LogInformation("Azure Cloud deployment ended at {DeploymentStartDateTime}. Duration: {DeploymentDuration}", startDateTime, endDateTime - startDateTime);
    }
}