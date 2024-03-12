// <copyright file="Dynamics365BusinessEventValidator.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.BusinessEvents;

/// <summary>
/// The dynamics365 business event metadata validator.
/// </summary>
public sealed class Dynamics365BusinessEventValidator : Dynamics365BusinessEventValidator<Dynamics365BusinessEventBase>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365BusinessEventValidator"/> class.
    /// </summary>
    public Dynamics365BusinessEventValidator()
    {
    }
}