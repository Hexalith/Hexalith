// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance
// Author           : Jérôme Piquot
// Created          : 10-24-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-14-2023
// ***********************************************************************
// <copyright file="CustomerGroupUpdate.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Services.CustomerGroups;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Class CustomerGroupUpdate.
/// </summary>
[DataContract]
public class CustomerGroupUpdate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerGroupUpdate"/> class.
    /// </summary>
    public CustomerGroupUpdate()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerGroupUpdate" /> class.
    /// </summary>
    /// <param name="clearingPeriodPaymentTermName">Name of the clearing period payment term.</param>
    /// <param name="defaultDimensionDisplayValue">The default dimension display value.</param>
    /// <param name="customerAccountNumberSequence">The customer account number sequence.</param>
    /// <param name="description">The description.</param>
    /// <param name="isSalesTaxIncludedInPrice">The is sales tax included in price.</param>
    /// <param name="writeOffReason">The write off reason.</param>
    /// <param name="paymentTermId">The payment term identifier.</param>
    /// <param name="taxGroupId">The tax group identifier.</param>
    /// <param name="isPublicSectorIT">The is public sector it.</param>
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
      string? isPublicSectorIT)
    {
        ClearingPeriodPaymentTermName = clearingPeriodPaymentTermName;
        DefaultDimensionDisplayValue = defaultDimensionDisplayValue;
        CustomerAccountNumberSequence = customerAccountNumberSequence;
        Description = description;
        IsSalesTaxIncludedInPrice = isSalesTaxIncludedInPrice;
        WriteOffReason = writeOffReason;
        PaymentTermId = paymentTermId;
        TaxGroupId = taxGroupId;
        IsPublicSectorIT = isPublicSectorIT;
    }

    /// <summary>
    /// Gets the name of the clearing period payment term.
    /// </summary>
    /// <value>The name of the clearing period payment term.</value>
    public string? ClearingPeriodPaymentTermName { get; init; }

    /// <summary>
    /// Gets the customer account number sequence.
    /// </summary>
    /// <value>The customer account number sequence.</value>
    public string? CustomerAccountNumberSequence { get; init; }

    /// <summary>
    /// Gets the default dimension display value.
    /// </summary>
    /// <value>The default dimension display value.</value>
    public string? DefaultDimensionDisplayValue { get; init; }

    /// <summary>
    /// Gets the description.
    /// </summary>
    /// <value>The description.</value>
    public string? Description { get; init; }

    /// <summary>
    /// Gets the is public sector it.
    /// </summary>
    /// <value>The is public sector it.</value>
    public string? IsPublicSectorIT { get; init; }

    /// <summary>
    /// Gets the is sales tax included in price.
    /// </summary>
    /// <value>The is sales tax included in price.</value>
    public string? IsSalesTaxIncludedInPrice { get; init; }

    /// <summary>
    /// Gets the payment term identifier.
    /// </summary>
    /// <value>The payment term identifier.</value>
    public string? PaymentTermId { get; init; }

    /// <summary>
    /// Gets the tax group identifier.
    /// </summary>
    /// <value>The tax group identifier.</value>
    public string? TaxGroupId { get; init; }

    /// <summary>
    /// Gets the write off reason.
    /// </summary>
    /// <value>The write off reason.</value>
    public string? WriteOffReason { get; init; }
}