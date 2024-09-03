// <copyright file="IRole.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Authorizations;

using System.Collections.Generic;

public interface IRole
{
    /// <summary>
    /// Gets the description of the duty.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Gets the collection of duties associated with the role.
    /// </summary>
    /// <value>The duties.</value>
    IEnumerable<IDuty> Duties { get; }

    /// <summary>
    /// Gets the ID of the duty.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets the name of the duty.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the collection of privileges associated with the role.
    /// </summary>
    IEnumerable<IPrivilege> Privileges { get; }
}