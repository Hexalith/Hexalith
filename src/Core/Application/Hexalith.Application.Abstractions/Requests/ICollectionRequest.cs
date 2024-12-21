// <copyright file="ICollectionRequest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

/// <summary>
/// Interface for requests.
/// </summary>
public interface ICollectionRequest
{
    /// <summary>
    /// Gets the results.
    /// </summary>
    IEnumerable<object>? Results { get; }
}