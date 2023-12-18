// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-21-2023
// ***********************************************************************
// <copyright file="Gender.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Domain.ValueObjets;

/// <summary>
/// Enum Gender.
/// </summary>
public enum Gender
{
    /// <summary>
    /// The male.
    /// </summary>
    Male,

    /// <summary>
    /// The female.
    /// </summary>
    Female,

    /// <summary>
    /// The non specific.
    /// </summary>
    NonSpecific,

    /// <summary>
    /// Unknown gender.
    /// </summary>
    Unknown,
}