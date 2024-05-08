// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 11-01-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-01-2023
// ***********************************************************************
// <copyright file="SalesInvoiceIssuedValidator.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Events;

using FluentValidation;

/// <summary>
/// Class SalesInvoiceRegisteredValidator.
/// </summary>
public class SalesInvoiceIssuedValidator : AbstractValidator<SalesInvoiceIssued>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SalesInvoiceIssuedValidator"/> class.
    /// </summary>
    public SalesInvoiceIssuedValidator()
    {
        _ = RuleFor(x => x.PartitionId).NotEmpty();
        _ = RuleFor(x => x.CompanyId).NotEmpty();
        _ = RuleFor(x => x.Id).NotEmpty();
    }
}