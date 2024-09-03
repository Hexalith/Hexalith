// ***********************************************************************
// Assembly         : Hexalith.Application.TopologicalSorting
// Author           : Jérôme Piquot
// Created          : 04-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-04-2023
// ***********************************************************************
// <copyright file="OrderedProcess.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.TopologicalSorting;

/// <summary>
/// A process that requires execution, a process depends upon other processes being executed first, and the resources it uses not being consumed at the same time.
/// </summary>
public class OrderedProcess
{
    /// <summary>
    /// The followers.
    /// </summary>
    private readonly HashSet<OrderedProcess> _followers = [];

    /// <summary>
    /// The predecessors.
    /// </summary>
    private readonly HashSet<OrderedProcess> _predecessors = [];

    /// <summary>
    /// The resources.
    /// </summary>
    private readonly HashSet<Resource> _resources = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderedProcess" /> class.
    /// </summary>
    /// <param name="graph">The graph which this process is part of.</param>
    /// <param name="name">The name of this process.</param>
    public OrderedProcess(DependencyGraph graph, string name)
    {
        Graph = graph;
        Name = name;

        _ = Graph.Add(this);
    }

    /// <summary>
    /// Gets the followers of this process.
    /// </summary>
    /// <value>The followers.</value>
    public IEnumerable<OrderedProcess> Followers => _followers;

    /// <summary>
    /// Gets the graph this process is part of.
    /// </summary>
    public DependencyGraph Graph { get; }

    /// <summary>
    /// Gets the name of this process.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the predecessors of this process.
    /// </summary>
    /// <value>The predecessors.</value>
    public IEnumerable<OrderedProcess> Predecessors => _predecessors;

    /// <summary>
    /// Gets the resources this process depends upon.
    /// </summary>
    /// <value>The resources.</value>
    public IEnumerable<Resource> Resources => _resources;

    /// <summary>
    /// Gets the resources set.
    /// </summary>
    /// <value>The resources set.</value>
    internal ISet<Resource> ResourcesSet => _resources;

    /// <summary>
    /// Indicates that this process should execute after another.
    /// </summary>
    /// <param name="predecessor">The predecessor.</param>
    /// <returns>returns this process.</returns>
    public OrderedProcess After(OrderedProcess predecessor)
    {
        ArgumentNullException.ThrowIfNull(predecessor);
        DependencyGraph.CheckGraph(this, predecessor);

        if (_predecessors.Add(predecessor))
        {
            _ = predecessor.Before(this);
        }

        return predecessor;
    }

    /// <summary>
    /// Indicates that this process must happen after all the predecessors.
    /// </summary>
    /// <param name="predecessors">The predecessors.</param>
    /// <returns>the predecessors.</returns>
    public IEnumerable<OrderedProcess> After(params OrderedProcess[] predecessors) => After(predecessors as IEnumerable<OrderedProcess>);

    /// <summary>
    /// Indicates that this process must happen after all the predecessors.
    /// </summary>
    /// <param name="predecessors">The predecessors.</param>
    /// <returns>the predecessors.</returns>
    public IEnumerable<OrderedProcess> After(IEnumerable<OrderedProcess> predecessors)
    {
        ArgumentNullException.ThrowIfNull(predecessors);
        foreach (OrderedProcess predecessor in predecessors)
        {
            _ = After(predecessor);
        }

        return predecessors;
    }

    /// <summary>
    /// Indicates that this process should execute before another.
    /// </summary>
    /// <param name="follower">The ancestor.</param>
    /// <returns>returns this process.</returns>
    public OrderedProcess Before(OrderedProcess follower)
    {
        ArgumentNullException.ThrowIfNull(follower);
        DependencyGraph.CheckGraph(this, follower);

        if (_followers.Add(follower))
        {
            _ = follower.After(this);
        }

        return follower;
    }

    /// <summary>
    /// Indicates that this process must happen before all the followers.
    /// </summary>
    /// <param name="followers">The followers.</param>
    /// <returns>the followers.</returns>
    public IEnumerable<OrderedProcess> Before(params OrderedProcess[] followers) => Before(followers as IEnumerable<OrderedProcess>);

    /// <summary>
    /// Indicates that this process must happen before all the followers.
    /// </summary>
    /// <param name="followers">The followers.</param>
    /// <returns>the followers.</returns>
    public IEnumerable<OrderedProcess> Before(IEnumerable<OrderedProcess> followers)
    {
        ArgumentNullException.ThrowIfNull(followers);
        foreach (OrderedProcess ancestor in followers)
        {
            _ = Before(ancestor);
        }

        return followers;
    }

    /// <summary>
    /// Indicates that this process requires the specified resource.
    /// </summary>
    /// <param name="resource">The resource.</param>
    public void Requires(Resource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        DependencyGraph.CheckGraph(resource, this);

        if (_resources.Add(resource))
        {
            resource.UsedBy(this);
        }
    }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString() => "Process { " + Name + " }";
}