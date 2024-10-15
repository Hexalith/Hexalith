// <copyright file="IPrivilege.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Authorizations;

/// <summary>
/// Represents a privilege in the application.
/// </summary>
public interface IPrivilege
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
}