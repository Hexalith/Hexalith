// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Parties
// Author           : jpiquot
// Created          : 02-26-2023
//
// Last Modified By : jpiquot
// Last Modified On : 02-26-2023
// ***********************************************************************
// <copyright file="Dynamics365FinanceCustomerRegisteredValidator.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.BusinessEvents;

using FluentValidation;

/// <summary>
/// Class FFYBspkCustomerValidator.
/// Implements the <see cref="AbstractValidator{FFYCustomerRegisteredBusinessEvent}" />.
/// </summary>
/// <seealso cref="AbstractValidator{FFYCustomerRegisteredBusinessEvent}" />
public class Dynamics365FinanceCustomerRegisteredValidator : AbstractValidator<Dynamics365FinanceCustomerRegistered>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceCustomerRegisteredValidator"/> class.
    /// </summary>
    public Dynamics365FinanceCustomerRegisteredValidator()
    {
        _ = RuleFor(x => x.BusinessEventLegalEntity)
           .NotEmpty();
        _ = RuleFor(x => x.Name)
           .NotEmpty();
        _ = RuleFor(x => x.Account)
           .NotEmpty();
    }
}