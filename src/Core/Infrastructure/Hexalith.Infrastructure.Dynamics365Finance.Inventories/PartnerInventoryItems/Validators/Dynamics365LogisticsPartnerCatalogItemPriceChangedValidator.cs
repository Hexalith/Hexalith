// ***********************************************************************
// Assembly         : Dynamics365LogisticsPartnerBridge.CatalogItems
// Author           : Jérôme Piquot
// Created          : 02-23-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 03-03-2023
// ***********************************************************************
// <copyright file="LogisticsPartnerCatalogItemChangePriceValidator.cs" company="Fiveforty SAS Paris France">
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
/// Class Dynamics365LogisticsPartnerCatalogItemChangePriceValidator.
/// Implements the <see cref="Dynamics365LogisticsPartnerCatalogCommandValidator{Dynamics365LogisticsPartnerCatalogItemChangePrice}" />.
/// </summary>
/// <seealso cref="Dynamics365LogisticsPartnerCatalogCommandValidator{Dynamics365LogisticsPartnerCatalogItemChangePrice}" />
public class Dynamics365LogisticsPartnerCatalogItemPriceChangedValidator : Dynamics365BusinessEventValidator<Dynamics365LogisticsPartnerCatalogItemPriceChanged>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365LogisticsPartnerCatalogItemPriceChangedValidator" /> class.
    /// </summary>
    public Dynamics365LogisticsPartnerCatalogItemPriceChangedValidator()
    {
        _ = RuleFor(x => x.NewPrice)
           .GreaterThanOrEqualTo(0);
    }
}