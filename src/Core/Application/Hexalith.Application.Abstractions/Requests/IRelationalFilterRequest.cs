// <copyright file="IRelationalFilterRequest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

/// <summary>
/// Represents a request that can be searched.
/// </summary>
public interface IRelationalFilterRequest : ISearchRequest
{
    /// <summary>
    /// Gets the search step.
    /// </summary>
    IDictionary<string, IEnumerable<string>>? RelationalFilters { get; }
}