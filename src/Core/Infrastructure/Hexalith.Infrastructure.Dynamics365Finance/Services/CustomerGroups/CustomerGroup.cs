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

using System.Runtime.Serialization;

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
[DataContract]
public record CustomerGroup(
      string Etag,
      string DataAreaId,
      [property: DataMember(Order = 3)] string CustomerGroupId,
      [property: DataMember(Order = 4)] string ClearingPeriodPaymentTermName,
      [property: DataMember(Order = 5)] string DefaultDimensionDisplayValue,
      [property: DataMember(Order = 6)] string CustomerAccountNumberSequence,
      [property: DataMember(Order = 7)] string Description,
      [property: DataMember(Order = 8)] string IsSalesTaxIncludedInPrice,
      [property: DataMember(Order = 9)] string WriteOffReason,
      [property: DataMember(Order = 10)] string PaymentTermId,
      [property: DataMember(Order = 11)] string TaxGroupId,
      [property: DataMember(Order = 12)] string IsPublicSectorIT) : ODataElement(Etag, DataAreaId), IODataElement
{
    /// <summary>
    /// Entities the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string EntityName() => "CustomerGroups";
}