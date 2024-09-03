// <copyright file="IDuty.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Authorizations;

/// <summary>
/// Represents a duty in the application.
/// </summary>
/// <remarks>
/// A duty defines a specific responsibility or role within the application.
/// It contains a description, an ID, a name, and a collection of privileges.
/// </remarks>
public interface IDuty
{
    /// <summary>
    /// Gets the description of the duty.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Gets the ID of the duty.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets the name of the duty.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the collection of privileges associated with the duty.
    /// </summary>
    IEnumerable<IPrivilege> Privileges { get; }
}