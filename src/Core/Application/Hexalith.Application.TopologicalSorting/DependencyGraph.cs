// <copyright file="DependencyGraph.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.TopologicalSorting;

using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A graph of processes and resources from which a topological sort can be extracted.
/// </summary>
public class DependencyGraph
{
    /// <summary>
    /// The processes.
    /// </summary>
    private readonly HashSet<OrderedProcess> _processes = [];

    /// <summary>
    /// The resources.
    /// </summary>
    private readonly HashSet<Resource> _resources = [];

    /// <summary>
    /// Gets the process count.
    /// </summary>
    /// <value>The process count.</value>
    public int ProcessCount => _processes.Count;

    /// <summary>
    /// Gets the processes which are part of this dependency graph.
    /// </summary>
    /// <value>The processes.</value>
    public IEnumerable<OrderedProcess> Processes => _processes;

    /// <summary>
    /// Gets the resources which are part of this dependency graph.
    /// </summary>
    /// <value>The resources.</value>
    public IEnumerable<Resource> Resources => _resources;

    /// <summary>
    /// Calculates the sort which results from this dependency network.
    /// </summary>
    /// <returns>TopologicalSort.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no sort exists for the given set of constraints.</exception>
    public TopologicalSort CalculateSort() => CalculateSort(new TopologicalSort());

    /// <summary>
    /// Append the result of this dependency graph to the end of the given sorting solution.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <returns>TopologicalSort.</returns>
    /// <exception cref="System.InvalidOperationException">Cannot order this set of processes.</exception>
    public TopologicalSort CalculateSort(TopologicalSort instance)
    {
        ArgumentNullException.ThrowIfNull(instance);
        HashSet<OrderedProcess> unused = [.. _processes];

        do
        {
            // select processes which have no predecessors in the unused set, which means that all their predecessors must either be used, or not exist, either way is fine
            HashSet<OrderedProcess> set = [.. unused.Where(p => !unused.Overlaps(p.Predecessors))];

            if (set.Count == 0)
            {
                throw new InvalidOperationException("Cannot order this set of processes");
            }

            unused.ExceptWith(set);

            foreach (ISet<OrderedProcess> subset in SolveResourceDependencies(set))
            {
                instance.Append(subset);
            }
        }
        while (unused.Count > 0);

        return instance;
    }

    /// <summary>
    /// Checks the graph.
    /// </summary>
    /// <param name="a">a.</param>
    /// <param name="b">The b.</param>
    /// <exception cref="System.ArgumentException">process {a} is not associated with the same graph as process {b}.</exception>
    internal static void CheckGraph(OrderedProcess a, OrderedProcess b)
    {
        if (a.Graph != b.Graph)
        {
            throw new ArgumentException($"process {a} is not associated with the same graph as process {b}.");
        }
    }

    /// <summary>
    /// Checks the graph.
    /// </summary>
    /// <param name="a">a.</param>
    /// <param name="b">The b.</param>
    /// <exception cref="System.ArgumentException">Resource {a} is not associated with the same graph as process {b}.</exception>
    internal static void CheckGraph(Resource a, OrderedProcess b)
    {
        if (a.Graph != b.Graph)
        {
            throw new ArgumentException($"Resource {a} is not associated with the same graph as process {b}.");
        }
    }

    /// <summary>
    /// Adds the specified ordered process.
    /// </summary>
    /// <param name="orderedProcess">The ordered process.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    internal bool Add(OrderedProcess orderedProcess) => _processes.Add(orderedProcess);

    /// <summary>
    /// Adds the specified resource class.
    /// </summary>
    /// <param name="resourceClass">The resource class.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    internal bool Add(Resource resourceClass) => _resources.Add(resourceClass);

    /// <summary>
    /// Given a set of processes which are not interdependent, split up into multiple sets which do not use the same resource concurrently.
    /// </summary>
    /// <param name="processes">The processes.</param>
    /// <returns>IEnumerable&lt;ISet&lt;OrderedProcess&gt;&gt;.</returns>
    private IEnumerable<ISet<OrderedProcess>> SolveResourceDependencies(ISet<OrderedProcess> processes)
    {
        // if there are no resources in this graph, or none of the processes in this set have any
        // resources, we can simply return the set of processes
        if (_resources.Count == 0 || !processes.SelectMany(p => p.ResourcesSet).Any())
        {
            yield return processes;
        }
        else
        {
            HashSet<HashSet<OrderedProcess>> result = [];

            foreach (OrderedProcess process in processes)
            {
                OrderedProcess process1 = process;

                // all sets this process may be added to
                IEnumerable<HashSet<OrderedProcess>> agreeableSets = result // from the set of result sets
                    .Where(set => set // select a candidate set to add to
                        .Where(p => p.ResourcesSet.Overlaps(process1.Resources)) // select processes whose resource usage overlaps this one
                        .IsEmpty());                                                            // if there are none which overlap, then this is a valid set

                // the single best set to add to
                HashSet<OrderedProcess> agreeableSet;

                if (agreeableSets.IsEmpty())
                {
                    // no sets can hold this process, create a new one
                    agreeableSet = [];
                    _ = result.Add(agreeableSet);
                }
                else
                {
                    agreeableSet = agreeableSets.Aggregate((a, b) => a.Count < b.Count ? a : b);    // pick the smallest set
                }

                // finally, add this process to the selected set
                _ = agreeableSet.Add(process);
            }

            foreach (HashSet<OrderedProcess> set in result)
            {
                yield return set;
            }
        }
    }
}