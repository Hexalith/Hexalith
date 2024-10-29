// <copyright file="StateMachine.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain;

using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents a generic state machine that manages valid state transitions.
/// This abstract class provides a foundation for implementing state machines with type-safe state transitions.
/// </summary>
/// <typeparam name="T">The type representing the states in the state machine. This type defines the possible states that can be managed.</typeparam>
/// <remarks>
/// The state machine ensures that only valid state transitions are allowed.
/// Derived classes must implement the ValidTransitions property to define the allowed state transitions.
/// </remarks>
public abstract record StateMachine<T>
{
    /// <summary>
    /// Gets the dictionary of valid state transitions.
    /// </summary>
    /// <value>
    /// A dictionary where the key represents the source state and the value is a collection of valid target states.
    /// Each entry defines the allowed transitions from a specific state.
    /// </value>
    protected abstract IDictionary<T, IEnumerable<T>> ValidTransitions { get; }

    /// <summary>
    /// Determines whether a transition from one state to another is valid.
    /// </summary>
    /// <param name="from">The source state.</param>
    /// <param name="to">The target state.</param>
    /// <returns><c>true</c> if the transition is valid; otherwise, <c>false</c>.</returns>
    public bool IsValidTransition(T from, T to) => GetValidTransitions(from).Contains(to);

    /// <summary>
    /// Gets all valid transitions from a given state.
    /// </summary>
    /// <param name="from">The source state to get valid transitions for.</param>
    /// <returns>An enumerable collection of valid next states. Returns an empty collection if no transitions are defined for the source state.</returns>
    public IEnumerable<T> GetValidTransitions(T from)
    {
        return !ValidTransitions.ContainsKey(from)
            ? []
            : ValidTransitions[from];
    }
}
