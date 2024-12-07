// <copyright file="IIdCollectionProjectionFactory.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Projections;

/// <summary>
/// Interface for creating actor projections.
/// </summary>
public interface IIdCollectionProjectionFactory : IProjectionFactory<IdCollection>
{
    /// <summary>
    /// Gets the collection domain name.
    /// </summary>
    public string CollectionDomainName { get; }
}