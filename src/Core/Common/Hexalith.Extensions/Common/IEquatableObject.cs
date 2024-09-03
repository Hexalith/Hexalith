// <copyright file="IEquatableObject.cs">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Common;

/// <summary>
/// Interface IEquatableObject.
/// </summary>
public interface IEquatableObject
{
    /// <summary>
    /// Gets the equality components.
    /// </summary>
    /// <returns>IEnumerable&lt;System.Nullable&lt;System.Object&gt;&gt;.</returns>
    IEnumerable<object?> GetEqualityComponents();
}