// ***********************************************************************
// Assembly         : Hexalith.Domain.ExternalSystems
// Author           : Jérôme Piquot
// Created          : 11-01-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-01-2023
// ***********************************************************************
// <copyright file="ExternalSystemReferenceRemovedValidator.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Events;

using FluentValidation;

/// <summary>
/// Class ExternalSystemReferenceRemovedValidator.
/// Implements the <see cref="FluentValidation.AbstractValidator{Hexalith.Domain.Events.ExternalSystemReferenceRemoved}" />.
/// </summary>
/// <seealso cref="FluentValidation.AbstractValidator{Hexalith.Domain.Events.ExternalSystemReferenceRemoved}" />
public class ExternalSystemReferenceRemovedValidator : AbstractValidator<ExternalSystemReferenceRemoved>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalSystemReferenceRemovedValidator" /> class.
    /// </summary>
    public ExternalSystemReferenceRemovedValidator()
    {
        _ = RuleFor(x => x.PartitionId).NotEmpty();
        _ = RuleFor(x => x.CompanyId).NotEmpty();
        _ = RuleFor(x => x.SystemId).NotEmpty();
        _ = RuleFor(x => x.ReferenceAggregateName).NotEmpty();
        _ = RuleFor(x => x.ExternalId).NotEmpty();
    }
}