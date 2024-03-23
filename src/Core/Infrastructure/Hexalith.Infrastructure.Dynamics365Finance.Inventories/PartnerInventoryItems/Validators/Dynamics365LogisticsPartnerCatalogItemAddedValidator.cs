// ***********************************************************************
// Assembly         : Dynamics365LogisticsPartnerBridge.CatalogItems
// Author           : Jérôme Piquot
// Created          : 02-23-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-13-2023
// ***********************************************************************
// <copyright file="Dynamics365FinancePartnerInventoryItemAddedValidator.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Inventories.PartnerInventoryItems.Validators;

using FluentValidation;

using Hexalith.Infrastructure.Dynamics365Finance.BusinessEvents;
using Hexalith.Infrastructure.Dynamics365Finance.Inventories.PartnerInventoryItems.BusinessEvents;

/// <summary>
/// Class Dynamics365FinancePartnerInventoryItemAddValidator.
/// Implements the <see cref="Dynamics365LogisticsPartnerCatalogCommandValidator{Dynamics365FinancePartnerInventoryItemAdd}" />.
/// </summary>
/// <seealso cref="Dynamics365LogisticsPartnerCatalogCommandValidator{Dynamics365FinancePartnerInventoryItemAdd}" />
public class Dynamics365FinancePartnerInventoryItemAddedValidator : Dynamics365BusinessEventValidator<Dynamics365FinancePartnerInventoryItemAdded>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinancePartnerInventoryItemAddedValidator"/> class.
    /// </summary>
    public Dynamics365FinancePartnerInventoryItemAddedValidator()
    {
        _ = RuleFor(x => x.LogisticsPartnerId)
           .NotEmpty();
        _ = RuleFor(x => x.BarCode)
           .NotEmpty();
        _ = RuleFor(x => x.RetailVariantId)
            .NotEmpty();
        _ = RuleFor(x => x.Name)
            .NotEmpty();
        _ = RuleFor(x => x.HarmonizedTariffScheduleCode)
            .NotEmpty();
        _ = RuleFor(x => x.CountryOfOriginCode)
            .Length(3) // Empty or must have 3 characters (ISO 3166‑1 alpha‑3 country code)
            .When(s => !string.IsNullOrWhiteSpace(s.CountryOfOriginCode));
    }
}