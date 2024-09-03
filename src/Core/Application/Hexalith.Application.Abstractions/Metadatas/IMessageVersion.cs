// <copyright file="IMessageVersion.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Metadatas;

/// <summary>
/// The message version.
/// </summary>
public interface IMessageVersion
{
    /// <summary>
    /// Gets the major version.
    /// </summary>
    int Major { get; }

    /// <summary>
    /// Gets the minor version.
    /// </summary>
    int Minor { get; }
}