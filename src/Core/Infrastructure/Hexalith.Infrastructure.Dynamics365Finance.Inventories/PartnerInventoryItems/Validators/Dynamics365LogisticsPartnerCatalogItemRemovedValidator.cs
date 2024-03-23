// ***********************************************************************
// Assembly         : Dynamics365LogisticsPartnerBridge.CatalogItems
// Author           : Jérôme Piquot
// Created          : 02-23-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 03-03-2023
// ***********************************************************************
// <copyright file="LogisticsPartnerCatalogItemRemoveValidator.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Inventories.PartnerInventoryItems.Validators;

using Hexalith.Infrastructure.Dynamics365Finance.BusinessEvents;
using Hexalith.Infrastructure.Dynamics365Finance.Inventories.PartnerInventoryItems.BusinessEvents;

/// <summary>
/// Class Dynamics365FinancePartnerInventoryItemRemoveValidator.
/// Implements the <see cref="Dynamics365LogisticsPartnerCatalogCommandValidator{Dynamics365FinancePartnerInventoryItemRemove}" />.
/// </summary>
/// <seealso cref="Dynamics365LogisticsPartnerCatalogCommandValidator{Dynamics365FinancePartnerInventoryItemRemove}" />
public class Dynamics365FinancePartnerInventoryItemRemovedValidator : Dynamics365BusinessEventValidator<Dynamics365FinancePartnerInventoryItemRemoved>
{
}