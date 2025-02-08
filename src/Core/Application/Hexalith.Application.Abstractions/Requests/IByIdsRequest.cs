// <copyright file="IByIdsRequest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

/// <summary>
/// Interface for requests that retrieve results by a list of identifiers.
/// </summary>
public interface IByIdsRequest : ICollectionRequest
{
    /// <summary>
    /// Gets the list of identifiers.
    /// </summary>
    IEnumerable<string> Ids { get; }
}