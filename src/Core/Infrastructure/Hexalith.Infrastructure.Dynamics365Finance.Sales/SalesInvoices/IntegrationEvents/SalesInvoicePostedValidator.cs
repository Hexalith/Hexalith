// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Sales
// Author           : jpiquot
// Created          : 02-26-2023
//
// Last Modified By : jpiquot
// Last Modified On : 02-26-2023
// ***********************************************************************
// <copyright file="SalesInvoicePostedValidator.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesInvoices.IntegrationEvents;

using FluentValidation;

/// <summary>
/// Class FFYBspkSalesInvoiceValidator.
/// Implements the <see cref="AbstractValidator{FFYSalesInvoiceRegisteredBusinessEvent}" />.
/// </summary>
/// <seealso cref="AbstractValidator{FFYSalesInvoiceRegisteredBusinessEvent}" />
public class SalesInvoicePostedValidator : AbstractValidator<SalesInvoicePostedBusinessEvent>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SalesInvoicePostedValidator"/> class.
    /// </summary>
    public SalesInvoicePostedValidator()
    {
        _ = RuleFor(x => x.BusinessEventLegalEntity)
           .NotEmpty();
        _ = RuleFor(x => x.InvoiceId)
           .NotEmpty();
        _ = RuleFor(x => x.InvoiceAccount)
           .NotEmpty();
    }
}