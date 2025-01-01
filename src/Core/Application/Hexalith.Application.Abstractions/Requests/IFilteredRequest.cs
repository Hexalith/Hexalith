// <copyright file="IFilteredRequest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

/// <summary>
/// Represents a request that can be filtered.
/// </summary>
public interface IFilteredRequest : ICollectionRequest
{
    /// <summary>
    /// Gets the filter string.
    /// </summary>
    string? Filter { get; }
}