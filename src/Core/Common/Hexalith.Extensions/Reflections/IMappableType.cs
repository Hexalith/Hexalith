// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Serialization
// Author           : Jérôme Piquot
// Created          : 03-07-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-12-2023
// ***********************************************************************
// <copyright file="IMappableType.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Extensions.Reflections;

/// <summary>
/// Interface ITypeMappable.
/// </summary>
public interface IMappableType
{
    /// <summary>
    /// Gets the name of the type.
    /// </summary>
    /// <value>The name of the type.</value>
    string TypeMapName { get; }
}