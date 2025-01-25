// <copyright file="IDataFragment.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.StreamStores;

/// <summary>
/// Persisted stream item interface.
/// </summary>
public interface IDataFragment
{
    /// <summary>
    /// Gets the data object.
    /// </summary>
    /// <value>The data.</value>
    object Data { get; }

    /// <summary>
    /// Gets the meta data object.
    /// </summary>
    /// <value>The metadata.</value>
    object Metadata { get; }
}

/// <summary>
/// Persisted stream item interface.
/// </summary>
/// <typeparam name="TData">The type of the t data.</typeparam>
/// <typeparam name="TMeta">The type of the t meta.</typeparam>
public interface IDataFragment<out TData, out TMeta> : IDataFragment
{
    /// <summary>
    /// Gets the data object.
    /// </summary>
    /// <value>The data.</value>
    new TData Data { get; }

    /// <summary>
    /// Gets the meta data object.
    /// </summary>
    /// <value>The metadata.</value>
    new TMeta Metadata { get; }
}