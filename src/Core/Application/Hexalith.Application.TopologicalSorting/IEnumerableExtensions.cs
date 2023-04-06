// ***********************************************************************
// Assembly         : Hexalith.Application.TopologicalSorting
// Author           : Jérôme Piquot
// Created          : 04-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-04-2023
// ***********************************************************************
// <copyright file="IEnumerableExtensions.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.TopologicalSorting
{
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
            _ = predecessor.Before(followers);

            return predecessor;
        }

        /// <summary>
        /// Indicates that all members of the enumerable must happen after all the predecessors.
        /// </summary>
        /// <param name="followers">The followers.</param>
        /// <param name="predecessors">The predecessors.</param>
        /// <returns>the predecessors.</returns>
        public static IEnumerable<OrderedProcess> After(this IEnumerable<OrderedProcess> followers, params OrderedProcess[] predecessors)
        {
            return followers.After(predecessors as IEnumerable<OrderedProcess>);
        }

        /// <summary>
        /// Indicates that all members of the enumerable must happen after all the predecessors.
        /// </summary>
        /// <param name="followers">The followers.</param>
        /// <param name="predecessors">The predecessors.</param>
        /// <returns>the predecessors.</returns>
        public static IEnumerable<OrderedProcess> After(this IEnumerable<OrderedProcess> followers, IEnumerable<OrderedProcess> predecessors)
        {
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
            _ = follower.After(predecessors);

            return follower;
        }

        /// <summary>
        /// Indicates that all members of the enumerable must happen before all the followers.
        /// </summary>
        /// <param name="predecessors">The predecessors.</param>
        /// <param name="followers">The followers.</param>
        /// <returns>the followers.</returns>
        public static IEnumerable<OrderedProcess> Before(this IEnumerable<OrderedProcess> predecessors, params OrderedProcess[] followers)
        {
            return predecessors.Before(followers as IEnumerable<OrderedProcess>);
        }

        /// <summary>
        /// Indicates that all members of the enumerable must happen before all the followers.
        /// </summary>
        /// <param name="predecessors">The predecessors.</param>
        /// <param name="followers">The followers.</param>
        /// <returns>the followers.</returns>
        public static IEnumerable<OrderedProcess> Before(this IEnumerable<OrderedProcess> predecessors, IEnumerable<OrderedProcess> followers)
        {
            foreach (OrderedProcess follower in followers)
            {
                _ = follower.After(predecessors);
            }

            return followers;
        }

        /// <summary>
        /// Determines whether the specified enumerable is empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e">The e.</param>
        /// <returns><c>true</c> if the specified e is empty; otherwise, <c>false</c>.</returns>
        internal static bool IsEmpty<T>(this IEnumerable<T> e)
        {
            return !e.GetEnumerator().MoveNext();
        }
    }
}