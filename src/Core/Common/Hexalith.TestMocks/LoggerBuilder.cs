// Fiveforty S.A. Paris France (2022)
namespace Hexalith.TestMocks;

using Microsoft.Extensions.Logging;

using Moq;

public class LoggerBuilder<T>
{
	public ILogger<T> Build() => BuildMock().Object;

	public Mock<ILogger<T>> BuildMock() => new();
}