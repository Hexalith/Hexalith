// ***********************************************************************
// Assembly         : Hexalith.Application.TopologicalSorting
// Author           : Jérôme Piquot
// Created          : 04-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-04-2023
// ***********************************************************************
// <copyright file="TopologicalSort.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.TopologicalSorting;

/// <summary>
/// Represents a sorting solution.
/// </summary>
public class TopologicalSort
        : IEnumerable<ISet<OrderedProcess>>, IEnumerable<OrderedProcess>
{
    /// <summary>
    /// The collections.
    /// </summary>
    private readonly List<ISet<OrderedProcess>> _collections = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="TopologicalSort" /> class.
    /// </summary>
    public TopologicalSort()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TopologicalSort" /> class.
    /// </summary>
    /// <param name="g">The graph to fill this sort with.</param>
    public TopologicalSort(DependencyGraph g)
        : this()
    {
        ArgumentNullException.ThrowIfNull(g);
        _ = g.CalculateSort(this);
    }

    /// <summary>
    /// Returns an enumerator that iterates through the processes.
    /// </summary>
    /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
    public System.Collections.IEnumerator GetEnumerator() => (this as IEnumerable<OrderedProcess>).GetEnumerator();

    /// <summary>
    /// Gets the enumerator which enumerates sets of processes, where a set can be executed in any order.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    IEnumerator<ISet<OrderedProcess>> IEnumerable<ISet<OrderedProcess>>.GetEnumerator() => _collections.GetEnumerator();

    /// <summary>
    /// Gets the enumerator which enumerates through the processes in an order to be executed.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    IEnumerator<OrderedProcess> IEnumerable<OrderedProcess>.GetEnumerator()
    {
        IEnumerable<IEnumerable<OrderedProcess>> collections = this;

        return collections.SelectMany(collection => collection).GetEnumerator();
    }

    /// <summary>
    /// Appends the specified collection.
    /// </summary>
    /// <param name="collection">The collection.</param>
    internal void Append(ISet<OrderedProcess> collection) => _collections.Add(collection);
}