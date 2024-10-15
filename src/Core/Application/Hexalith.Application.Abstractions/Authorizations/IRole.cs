// <copyright file="IRole.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Authorizations;

using System.Collections.Generic;

/// <summary>
/// Represents a role in the application's authorization system.
/// </summary>
public interface IRole
{
    /// <summary>
    /// Gets the description of the role.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Gets the collection of duties associated with the role.
    /// </summary>
    IEnumerable<IDuty> Duties { get; }

    /// <summary>
    /// Gets the unique identifier of the role.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets the name of the role.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the collection of privileges associated with the role.
    /// </summary>
    IEnumerable<IPrivilege> Privileges { get; }
}