// <copyright file="CommandTask.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.TaskManager;

/// <summary>
/// Command task.
/// </summary>
public class CommandTask
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandTask" /> class.
    /// </summary>
    /// <param name="command">The command to process.</param>
    /// <param name="arguments"></param>
    /// <param name="workingDirectory"></param>
    /// <param name="environment"></param>
    public CommandTask(string command)
    {
        Command = command;
    }

    /// <summary>
    /// Gets the command to process.
    /// </summary>
    public string Command { get; }
}