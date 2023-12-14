// <copyright file="IODataElement.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Interface IODataElement
/// Extends the <see cref="Hexalith.Infrastructure.Dynamics365Finance.Models.IODataCommon" />.
/// </summary>
/// <seealso cref="Hexalith.Infrastructure.Dynamics365Finance.Models.IODataCommon" />
public interface IODataElement : IODataCommon
{
    /// <summary>
    /// Gets the company.
    /// </summary>
    string DataAreaId { get; }
}