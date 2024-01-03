// ***********************************************************************
// Assembly         : Hexalith.Extensions
// Author           : Jérôme Piquot
// Created          : 01-03-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-03-2024
// ***********************************************************************
// <copyright file="IEquatableObject.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

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