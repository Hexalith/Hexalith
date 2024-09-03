// ***********************************************************************
// Assembly         : Hexalith.Application.TopologicalSorting
// Author           : Jérôme Piquot
// Created          : 04-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-04-2023
// ***********************************************************************
// <copyright file="Resource.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.TopologicalSorting;

/// <summary>
/// A class of resource which may be used by a single concurrent process.
/// </summary>
public class Resource
{
    /// <summary>
    /// The users.
    /// </summary>
    private readonly HashSet<OrderedProcess> _users = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="Resource" /> class.
    /// </summary>
    /// <param name="graph">The graph which this ResourceClass is part of.</param>
    /// <param name="name">The name of this resource.</param>
    public Resource(DependencyGraph graph, string name)
    {
        Graph = graph;
        Name = name;

        _ = Graph.Add(this);
    }

    /// <summary>
    /// Gets the graph this class is part of.
    /// </summary>
    public DependencyGraph Graph { get; }

    /// <summary>
    /// Gets the name of this resource.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets a set of processes which use this resource.
    /// </summary>
    /// <value>The users.</value>
    public IEnumerable<OrderedProcess> Users => _users;

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString() => "Resource { " + Name + " }";

    /// <summary>
    /// Indicates that this resource is used by the given process.
    /// </summary>
    /// <param name="process">The process.</param>
    public void UsedBy(OrderedProcess process)
    {
        ArgumentNullException.ThrowIfNull(process);
        DependencyGraph.CheckGraph(this, process);

        if (_users.Add(process))
        {
            process.Requires(this);
        }
    }

    /// <summary>
    /// Indicates that this resource is used by the given processes.
    /// </summary>
    /// <param name="processes">The processes.</param>
    public void UsedBy(params OrderedProcess[] processes) => UsedBy(processes as IEnumerable<OrderedProcess>);

    /// <summary>
    /// Indicates that this resource is used by the given processes.
    /// </summary>
    /// <param name="processes">The processes.</param>
    public void UsedBy(IEnumerable<OrderedProcess> processes)
    {
        ArgumentNullException.ThrowIfNull(processes);
        foreach (OrderedProcess process in processes)
        {
            UsedBy(process);
        }
    }
}