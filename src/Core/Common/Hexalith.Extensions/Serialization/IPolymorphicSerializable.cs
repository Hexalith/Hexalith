// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Serialization
// Author           : Jérôme Piquot
// Created          : 03-07-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 03-10-2023
// ***********************************************************************
// <copyright file="IPolimorphicSerializable.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Abstractions.Converters;

/// <summary>
/// Interface IPolymorphicSerializable.
/// </summary>
public interface IPolymorphicSerializable
{
    /// <summary>
    /// Gets the major version.
    /// </summary>
    /// <value>The major version.</value>
    int MajorVersion { get; }

    /// <summary>
    /// Gets the minor version.
    /// </summary>
    /// <value>The minor version.</value>
    int MinorVersion { get; }

    /// <summary>
    /// Gets the name of the type.
    /// </summary>
    /// <value>The name of the type.</value>
    string TypeName { get; }
}