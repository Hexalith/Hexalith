// ***********************************************************************
// Assembly         : Hexalith.Extensions
// Author           : JérômePiquot
// Created          : 01-17-2023
//
// Last Modified By : JérômePiquot
// Last Modified On : 01-17-2023
// ***********************************************************************
// <copyright file="ErrorType.cs" company="Fiveforty">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Extensions.Common;

/// <summary>
/// Enum ErrorType.
/// </summary>
public enum ErrorCategory
{
    /// <summary>
    /// Unknown error type.
    /// </summary>
    Unknown,

    /// <summary>
    /// Functional error type.
    /// </summary>
    Functional,

    /// <summary>
    /// Technical error type.
    /// </summary>
    Technical,
}