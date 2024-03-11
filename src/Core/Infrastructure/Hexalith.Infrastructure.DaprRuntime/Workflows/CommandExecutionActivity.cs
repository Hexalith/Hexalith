// <copyright file="CommandExecutionActivity.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Workflows;

using System.Threading.Tasks;

using Dapr.Workflow;

public class CommandExecutionActivity : WorkflowActivity<CommandPayload, CommandResult>
{
    /// <inheritdoc/>
    public override Task<CommandResult> RunAsync(WorkflowActivityContext context, CommandPayload input) => Task.FromResult(new CommandResult());
}