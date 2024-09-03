// ***********************************************************************
// Assembly         : Hexalith.Application.TopologicalSorting
// Author           : Jérôme Piquot
// Created          : 04-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-04-2023
// ***********************************************************************
// <copyright file="IEnumerableExtensions.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.TopologicalSorting;

/// <summary>
/// Extensions to IEnumerable.
/// </summary>
public static class IEnumerableExtensions
{
    /// <summary>
    /// Indicates that all the members of the enumerable must happen after the single predecessor.
    /// </summary>
    /// <param name="followers">The followers.</param>
    /// <param name="predecessor">The predecessor.</param>
    /// <returns>the predecessor.</returns>
    public static OrderedProcess After(this IEnumerable<OrderedProcess> followers, OrderedProcess predecessor)
    {
        ArgumentNullException.ThrowIfNull(followers);
        ArgumentNullException.ThrowIfNull(predecessor);
        _ = predecessor.Before(followers);

        return predecessor;
    }

    /// <summary>
    /// Indicates that all members of the enumerable must happen after all the predecessors.
    /// </summary>
    /// <param name="followers">The followers.</param>
    /// <param name="predecessors">The predecessors.</param>
    /// <returns>the predecessors.</returns>
    public static IEnumerable<OrderedProcess> After(this IEnumerable<OrderedProcess> followers, params OrderedProcess[] predecessors) => followers.After(predecessors as IEnumerable<OrderedProcess>);

    /// <summary>
    /// Indicates that all members of the enumerable must happen after all the predecessors.
    /// </summary>
    /// <param name="followers">The followers.</param>
    /// <param name="predecessors">The predecessors.</param>
    /// <returns>the predecessors.</returns>
    public static IEnumerable<OrderedProcess> After(this IEnumerable<OrderedProcess> followers, IEnumerable<OrderedProcess> predecessors)
    {
        ArgumentNullException.ThrowIfNull(followers);
        ArgumentNullException.ThrowIfNull(predecessors);
        foreach (OrderedProcess predecessor in predecessors)
        {
            _ = predecessor.Before(followers);
        }

        return predecessors;
    }

    /// <summary>
    /// Indicates that all members of the enumerable must happen before the single follower.
    /// </summary>
    /// <param name="predecessors">The predecessors.</param>
    /// <param name="follower">The follower.</param>
    /// <returns>the followers.</returns>
    public static OrderedProcess Before(this IEnumerable<OrderedProcess> predecessors, OrderedProcess follower)
    {
        ArgumentNullException.ThrowIfNull(predecessors);
        ArgumentNullException.ThrowIfNull(follower);
        _ = follower.After(predecessors);

        return follower;
    }

    /// <summary>
    /// Indicates that all members of the enumerable must happen before all the followers.
    /// </summary>
    /// <param name="predecessors">The predecessors.</param>
    /// <param name="followers">The followers.</param>
    /// <returns>the followers.</returns>
    public static IEnumerable<OrderedProcess> Before(this IEnumerable<OrderedProcess> predecessors, params OrderedProcess[] followers) => predecessors.Before(followers as IEnumerable<OrderedProcess>);

    /// <summary>
    /// Indicates that all members of the enumerable must happen before all the followers.
    /// </summary>
    /// <param name="predecessors">The predecessors.</param>
    /// <param name="followers">The followers.</param>
    /// <returns>the followers.</returns>
    public static IEnumerable<OrderedProcess> Before(this IEnumerable<OrderedProcess> predecessors, IEnumerable<OrderedProcess> followers)
    {
        ArgumentNullException.ThrowIfNull(predecessors);
        ArgumentNullException.ThrowIfNull(followers);
        foreach (OrderedProcess follower in followers)
        {
            _ = follower.After(predecessors);
        }

        return followers;
    }

    /// <summary>
    /// Determines whether the specified e is empty.
    /// </summary>
    /// <typeparam name="T">Type of the list element.</typeparam>
    /// <param name="e">The e.</param>
    /// <returns><c>true</c> if the specified e is empty; otherwise, <c>false</c>.</returns>
    internal static bool IsEmpty<T>(this IEnumerable<T> e) => !e.GetEnumerator().MoveNext();
}