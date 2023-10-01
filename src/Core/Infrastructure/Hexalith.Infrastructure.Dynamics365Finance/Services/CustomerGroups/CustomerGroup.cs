// <copyright file="CustomerGroup.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Services.CustomerGroups;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Class CustomerGroup.
/// Implements the <see cref="ODataElement" />
/// Implements the <see cref="IODataElement" />.
/// </summary>
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
      string IsPublicSector_IT) : ODataElement(Etag, DataAreaId), IODataElement
{
    /// <summary>
    /// Entities the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string EntityName()
    {
        return "CustomerGroups";
    }
}