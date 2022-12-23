// <copyright file="CustomerGroupCreate.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Services.CustomerGroups;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

[DataContract]
public class CustomerGroupCreate
{
    [JsonConstructor]
    public CustomerGroupCreate(
      string dataAreaId,
      string customerGroupId,
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
        DataAreaId = dataAreaId;
        CustomerGroupId = customerGroupId;
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

    public string? ClearingPeriodPaymentTermName { get; }

    public string? CustomerAccountNumberSequence { get; }

    public string CustomerGroupId { get; }

    [JsonPropertyName("dataAreaId")]
    public string DataAreaId { get; }

    public string? DefaultDimensionDisplayValue { get; }

    public string? Description { get; }

    public string? IsPublicSector_IT { get; }

    public string? IsSalesTaxIncludedInPrice { get; }

    public string? PaymentTermId { get; }

    public string? TaxGroupId { get; }

    public string? WriteOffReason { get; }
}
