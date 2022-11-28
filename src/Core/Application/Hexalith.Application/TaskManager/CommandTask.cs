// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Application.TaskManager;

/// <summary>
/// Command task
/// </summary>
public class CommandTask
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CommandTask" /> class.
	/// </summary>
	/// <param name="command">The command to process</param>
	/// <param name="arguments"></param>
	/// <param name="workingDirectory"></param>
	/// <param name="environment"></param>
	public CommandTask(string command) => Command = command;

	/// <summary>
	/// The command to process
	/// </summary>
	public string Command { get; }
}