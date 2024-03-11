// <copyright file="CommandProcessorWorkflow.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Workflows;

using System.Threading.Tasks;

using Dapr.Workflow;

public class CommandProcessorWorkflow : Workflow<CommandProcessorPayload, CommandProcessorResult>
{
    /// <inheritdoc/>
    public override async Task<CommandProcessorResult> RunAsync(WorkflowContext context, CommandProcessorPayload input)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(input);
        CommandResult result = await context.CallActivityAsync<CommandResult>(nameof(CommandExecutionActivity), new CommandPayload()).ConfigureAwait(false);
        if (result?.Completed == true)
        {
            return new CommandProcessorResult(result.LastCommandId, 0, null);
        }

        await context.CreateTimer(TimeSpan.FromSeconds(5)).ConfigureAwait(false);
        CommandProcessorPayload continuePayload = new(input, input.RetryCount + 1);
        context.ContinueAsNew(continuePayload);
        return new CommandProcessorResult(result.LastCommandId, continuePayload.RetryCount, continuePayload.RetryTime);
    }
}