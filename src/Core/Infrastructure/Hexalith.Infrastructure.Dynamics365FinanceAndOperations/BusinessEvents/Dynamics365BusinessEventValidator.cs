// <copyright file="Dynamics365BusinessEventMetadataValidator.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.BusinessEvents;

using FluentValidation;

/// <summary>
/// The dynamics365 business event metadata validator.
/// </summary>
public class Dynamics365BusinessEventValidator : AbstractValidator<Dynamics365BusinessEventBase>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365BusinessEventValidator" /> class.
    /// </summary>
    public Dynamics365BusinessEventValidator()
    {
        _ = RuleFor(x => x.BusinessEventId)
          .NotEmpty();
        _ = RuleFor(x => x.InitiatingUserAzureActiveDirectoryObjectId)
            .NotEmpty();
        _ = RuleFor(x => x.EventId)
            .NotEmpty();
        _ = RuleFor(x => x.EventTime)
            .NotEmpty();
        _ = RuleFor(x => x.BusinessEventLegalEntity)
            .NotEmpty();
    }
}
