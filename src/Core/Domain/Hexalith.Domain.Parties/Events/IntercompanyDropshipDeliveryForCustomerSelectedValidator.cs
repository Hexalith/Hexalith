// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 11-01-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-01-2023
// ***********************************************************************
// <copyright file="IntercompanyDropshipDeliveryForCustomerSelectedValidator.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Domain.Events;

using FluentValidation;

/// <summary>
/// Class IntercompanyDropshipDeliveryForCustomerSelectedValidator.
/// Implements the <see cref="FluentValidation.AbstractValidator{Hexalith.Domain.Events.IntercompanyDropshipDeliveryForCustomerSelected}" />.
/// </summary>
/// <seealso cref="FluentValidation.AbstractValidator{Hexalith.Domain.Events.IntercompanyDropshipDeliveryForCustomerSelected}" />
public class IntercompanyDropshipDeliveryForCustomerSelectedValidator : AbstractValidator<IntercompanyDropshipDeliveryForCustomerSelected>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IntercompanyDropshipDeliveryForCustomerSelectedValidator"/> class.
    /// </summary>
    public IntercompanyDropshipDeliveryForCustomerSelectedValidator()
    {
        _ = RuleFor(x => x.PartitionId).NotEmpty();
        _ = RuleFor(x => x.CompanyId).NotEmpty();
        _ = RuleFor(x => x.Id).NotEmpty();
    }
}