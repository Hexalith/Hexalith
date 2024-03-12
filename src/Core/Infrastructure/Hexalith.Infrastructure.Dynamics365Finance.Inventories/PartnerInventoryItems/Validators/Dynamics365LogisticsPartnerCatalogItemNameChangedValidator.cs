// ***********************************************************************
// Assembly         : Dynamics365LogisticsPartnerBridge.CatalogItems
// Author           : Jérôme Piquot
// Created          : 02-23-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-13-2023
// ***********************************************************************
// <copyright file="LogisticsPartnerCatalogItemChangeNameValidator.cs" company="Fiveforty SAS Paris France">
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
/// Class Dynamics365LogisticsPartnerCatalogItemChangeNameValidator.
/// Implements the <see cref="Dynamics365LogisticsPartnerCatalogCommandValidator{Dynamics365LogisticsPartnerCatalogItemChangeName}" />.
/// </summary>
/// <seealso cref="Dynamics365LogisticsPartnerCatalogCommandValidator{Dynamics365LogisticsPartnerCatalogItemChangeName}" />
public class Dynamics365LogisticsPartnerCatalogItemNameChangedValidator : Dynamics365BusinessEventValidator<Dynamics365LogisticsPartnerCatalogItemNameChanged>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365LogisticsPartnerCatalogItemNameChangedValidator" /> class.
    /// </summary>
    public Dynamics365LogisticsPartnerCatalogItemNameChangedValidator()
    {
        _ = RuleFor(x => x.NewName)
            .NotEmpty()
            .Length(1, 200);
    }
}