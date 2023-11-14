// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance
// Author           : Jérôme Piquot
// Created          : 10-24-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-14-2023
// ***********************************************************************
// <copyright file="CustomerGroup.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Services.CustomerGroups;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Class CustomerGroup.
/// Implements the <see cref="ODataElement" />
/// Implements the <see cref="System.IEquatable{Hexalith.Infrastructure.Dynamics365Finance.Models.ODataElement}" />
/// Implements the <see cref="IODataElement" />
/// Implements the <see cref="System.IEquatable{Hexalith.Infrastructure.Dynamics365Finance.Services.CustomerGroups.CustomerGroup}" />.
/// </summary>
/// <seealso cref="ODataElement" />
/// <seealso cref="System.IEquatable{Hexalith.Infrastructure.Dynamics365Finance.Models.ODataElement}" />
/// <seealso cref="IODataElement" />
/// <seealso cref="System.IEquatable{Hexalith.Infrastructure.Dynamics365Finance.Services.CustomerGroups.CustomerGroup}" />
public record CustomerGroup(
      string Etag,
      string DataAreaId,
      string CustomerGroupId,
      string ClearingPeriodPaymentTermName,
      string DefaultDimensionDisplayValue,
      string CustomerAccountNumberSequence,
      string Description,
      string IsSalesTaxIncludedInPrice,
      string WriteOffReason,
      string PaymentTermId,
      string TaxGroupId,
      string IsPublicSectorIT) : ODataElement(Etag, DataAreaId), IODataElement
{
    /// <summary>
    /// Entities the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string EntityName() => "CustomerGroups";
}