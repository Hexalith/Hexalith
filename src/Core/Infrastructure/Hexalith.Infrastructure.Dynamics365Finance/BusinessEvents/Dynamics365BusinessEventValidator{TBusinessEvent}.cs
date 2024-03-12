// <copyright file="Dynamics365BusinessEventValidator{TBusinessEvent}.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.BusinessEvents;

using FluentValidation;

/// <summary>
/// The dynamics365 business event metadata validator.
/// </summary>
public class Dynamics365BusinessEventValidator<TBusinessEvent>
    : AbstractValidator<TBusinessEvent>
        where TBusinessEvent : Dynamics365BusinessEventBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365BusinessEventValidator{TBusinessEvent}"/> class.
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
            .NotNull()
            .WithMessage($"Message date and time must be defined using fields {nameof(Dynamics365BusinessEventBase.EventTime)} or {nameof(Dynamics365BusinessEventBase.EventTimeIso8601)}.")
            .When(x => x.EventTimeIso8601 == null);

        _ = RuleFor(x => x.EventTimeIso8601)
            .NotNull()
            .WithMessage($"Message date and time must be defined using fields {nameof(Dynamics365BusinessEventBase.EventTime)} or {nameof(Dynamics365BusinessEventBase.EventTimeIso8601)}.")
            .When(x => x.EventTime == null);

        _ = RuleFor(x => x.BusinessEventLegalEntity)
            .NotEmpty();
    }
}