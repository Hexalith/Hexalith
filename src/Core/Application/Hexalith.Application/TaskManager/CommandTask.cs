// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Application.TaskManager;

public class CommandTask
{
	public CommandTask(string command, string arguments, string workingDirectory, string? environment = null)
	{
		Command = command;
		Arguments = arguments;
		WorkingDirectory = workingDirectory;
		Environment = environment;
	}

	public string Arguments { get; }
	public string Command { get; }
	public string? Environment { get; }
	public string WorkingDirectory { get; }
}