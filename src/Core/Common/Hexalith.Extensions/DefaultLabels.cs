// ***********************************************************************
// Assembly         : Hexalith.Extensions
// Author           : Jérôme Piquot
// Created          : 08-28-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-28-2023
// ***********************************************************************
// <copyright file="DefaultLabels.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Extensions;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Class DefaultLabels.
/// </summary>
[SuppressMessage(
    "Critical Code Smell",
    "S2339:Public constant members should not be used",
    Justification = "Used in attributes that need constant values")]
public static class DefaultLabels
{
    /// <summary>
    /// This constructor is only for serialization purposes message.
    /// </summary>
    public const string ForSerializationOnly = "This constructor is only for serialization purposes.";
}