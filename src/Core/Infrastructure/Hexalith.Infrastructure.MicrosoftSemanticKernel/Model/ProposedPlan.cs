// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.MicrosoftSemanticKernel
// Author           : Jérôme Piquot
// Created          : 05-31-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-30-2023
// ***********************************************************************
// <copyright file="ProposedPlan.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************.

namespace Hexalith.Infrastructure.MicrosoftSemanticKernel.Model;

using System.Text.Json.Serialization;

using Microsoft.SemanticKernel.Planning;

/// <summary>
/// Information about a single proposed plan.
/// </summary>
public class ProposedPlan
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProposedPlan"/> class.
    /// Create a new proposed plan.
    /// </summary>
    /// <param name="plan">Proposed plan object.</param>
    public ProposedPlan(Plan plan) => Plan = plan;

    /// <summary>
    /// Gets or sets plan object to be approved or invoked.
    /// </summary>
    /// <value>The plan.</value>
    [JsonPropertyName("proposedPlan")]
    public Plan Plan { get; set; }
}