// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.MicrosoftSemanticKernel
// Author           : Jérôme Piquot
// Created          : 05-31-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-31-2023
// ***********************************************************************
// <copyright file="CopilotChatPlanner.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.MicrosoftSemanticKernel.Skills.ChatSkills;

using System.Threading.Tasks;

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planning;
using Microsoft.SemanticKernel.SkillDefinition;

/// <summary>
/// A lightweight wrapper around a planner to allow for curating which skills are available to it.
/// </summary>
public class CopilotChatPlanner
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CopilotChatPlanner" /> class.
    /// </summary>
    /// <param name="plannerKernel">The planner's kernel.</param>
    public CopilotChatPlanner(IKernel plannerKernel) => Kernel = plannerKernel;

    /// <summary>
    /// Gets the planner's kernel.
    /// </summary>
    /// <value>The kernel.</value>
    public IKernel Kernel { get; }

    /// <summary>
    /// Create a plan for a goal.
    /// </summary>
    /// <param name="goal">The goal to create a plan for.</param>
    /// <returns>The plan.</returns>
    public Task<Plan> CreatePlanAsync(string goal)
    {
        FunctionsView plannerFunctionsView = Kernel.Skills.GetFunctionsView(true, true);
        if (plannerFunctionsView.NativeFunctions.IsEmpty && plannerFunctionsView.SemanticFunctions.IsEmpty)
        {
            // No functions are available - return an empty plan.
            return Task.FromResult(new Plan(goal));
        }

        return new ActionPlanner(Kernel).CreatePlanAsync(goal);
    }
}