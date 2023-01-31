// ***********************************************************************
// Assembly         : Hexalith.Extensions
// Author           : Jérôme Piquot
// Created          : 01-31-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-31-2023
// ***********************************************************************
// <copyright file="IIdempotent.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Extensions.Common;

/// <summary>
/// Interface IIdempotent.
/// </summary>
public interface IIdempotent
{
    /// <summary>
    /// Gets the idempotency identifier.
    /// </summary>
    /// <value>The idempotency identifier.</value>
    public string IdempotencyId { get; }
}