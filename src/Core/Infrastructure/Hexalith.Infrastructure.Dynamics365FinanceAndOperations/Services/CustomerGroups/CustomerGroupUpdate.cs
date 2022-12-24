// <copyright file="CustomerGroupUpdate.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Services.CustomerGroups;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

[DataContract]
public class CustomerGroupUpdate
{
    public CustomerGroupUpdate()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerGroupUpdate"/> class.
    /// </summary>
    /// <param name="clearingPeriodPaymentTermName"></param>
    /// <param name="defaultDimensionDisplayValue"></param>
    /// <param name="customerAccountNumberSequence"></param>
    /// <param name="description"></param>
    /// <param name="isSalesTaxIncludedInPrice"></param>
    /// <param name="writeOffReason"></param>
    /// <param name="paymentTermId"></param>
    /// <param name="taxGroupId"></param>
    /// <param name="isPublicSector_IT"></param>
    [JsonConstructor]
    public CustomerGroupUpdate(
      string? clearingPeriodPaymentTermName,
      string? defaultDimensionDisplayValue,
      string? customerAccountNumberSequence,
      string? description,
      string? isSalesTaxIncludedInPrice,
      string? writeOffReason,
      string? paymentTermId,
      string? taxGroupId,
      string? isPublicSector_IT)
    {
        ClearingPeriodPaymentTermName = clearingPeriodPaymentTermName;
        DefaultDimensionDisplayValue = defaultDimensionDisplayValue;
        CustomerAccountNumberSequence = customerAccountNumberSequence;
        Description = description;
        IsSalesTaxIncludedInPrice = isSalesTaxIncludedInPrice;
        WriteOffReason = writeOffReason;
        PaymentTermId = paymentTermId;
        TaxGroupId = taxGroupId;
        IsPublicSector_IT = isPublicSector_IT;
    }

    public string? ClearingPeriodPaymentTermName { get; init; }

    public string? CustomerAccountNumberSequence { get; init; }

    public string? DefaultDimensionDisplayValue { get; init; }

    public string? Description { get; init; }

    public string? IsPublicSector_IT { get; init; }

    public string? IsSalesTaxIncludedInPrice { get; init; }

    public string? PaymentTermId { get; init; }

    public string? TaxGroupId { get; init; }

    public string? WriteOffReason { get; init; }
}
