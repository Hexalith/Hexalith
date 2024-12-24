// <copyright file="IIdDescription.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Services;

/// <summary>
/// Interface for objects that have an ID and a description.
/// </summary>
public interface IIdDescription
{
    /// <summary>
    /// Gets the description of the object.
    /// </summary>
    /// <value>
    /// The description of the object.
    /// </value>
    string Description { get; }

    /// <summary>
    /// Gets the ID of the object.
    /// </summary>
    /// <value>
    /// The ID of the object.
    /// </value>
    string Id { get; }
}