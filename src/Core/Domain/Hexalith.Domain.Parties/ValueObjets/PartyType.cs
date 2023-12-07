// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 12-07-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-07-2023
// ***********************************************************************
// <copyright file="PartyType.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Domain.ValueObjets;

/// <summary>
/// Enum PartyType.
/// </summary>
public enum PartyType
{
    /// <summary>
    /// The person.
    /// </summary>
    Person,

    /// <summary>
    /// The organisation.
    /// </summary>
    Organisation,

    /// <summary>
    /// The other.
    /// </summary>
    Other,
}