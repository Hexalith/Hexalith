// <copyright file="CustomerGroupCreate.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Services.CustomerGroups;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Represents a customer group creation request.
/// </summary>
/// <remarks>
/// This class is used to create a new customer group in the Dynamics 365 Finance system.
/// </remarks>
[DataContract]
[method: JsonConstructor]
public class CustomerGroupCreate(
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
  string? isPublicSector)
{
    /// <summary>
    /// Gets the clearing period payment term name.
    /// </summary>
    public string? ClearingPeriodPaymentTermName { get; } = clearingPeriodPaymentTermName;

    /// <summary>
    /// Gets the customer account number sequence.
    /// </summary>
    public string? CustomerAccountNumberSequence { get; } = customerAccountNumberSequence;

    /// <summary>
    /// Gets the customer group ID.
    /// </summary>
    public string CustomerGroupId { get; } = customerGroupId;

    /// <summary>
    /// Gets the data area ID.
    /// </summary>
    [JsonPropertyName("dataAreaId")]
    public string DataAreaId { get; } = dataAreaId;

    /// <summary>
    /// Gets the default dimension display value.
    /// </summary>
    public string? DefaultDimensionDisplayValue { get; } = defaultDimensionDisplayValue;

    /// <summary>
    /// Gets the description.
    /// </summary>
    public string? Description { get; } = description;

    /// <summary>
    /// Gets the flag indicating whether the customer group is for the public sector IT.
    /// </summary>
    [JsonPropertyName("IsPublicSector_IT")]
    public string? IsPublicSector { get; } = isPublicSector;

    /// <summary>
    /// Gets the flag indicating whether sales tax is included in the price.
    /// </summary>
    public string? IsSalesTaxIncludedInPrice { get; } = isSalesTaxIncludedInPrice;

    /// <summary>
    /// Gets the payment term ID.
    /// </summary>
    public string? PaymentTermId { get; } = paymentTermId;

    /// <summary>
    /// Gets the tax group ID.
    /// </summary>
    public string? TaxGroupId { get; } = taxGroupId;

    /// <summary>
    /// Gets the write-off reason.
    /// </summary>
    public string? WriteOffReason { get; } = writeOffReason;
}