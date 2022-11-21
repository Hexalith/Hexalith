// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.TestMocks;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Configurations;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Security;
using Hexalith.TestMocks;

using Moq;

public class Dynamics365FinanceAndOperationsSecurityContextBuilder
{
	private LoggerBuilder<Dynamics365FinanceAndOperationsSecurityContext>? _logger = null;
	private OptionsBuilder<Dynamics365FinanceAndOperationsClientSettings>? _settings = null;

	public LoggerBuilder<Dynamics365FinanceAndOperationsSecurityContext> Logger
		=> _logger ??= new LoggerBuilder<Dynamics365FinanceAndOperationsSecurityContext>();

	public OptionsBuilder<Dynamics365FinanceAndOperationsClientSettings> Settings
			=> _settings ??= new OptionsBuilder<Dynamics365FinanceAndOperationsClientSettings>();

	public IDynamics365FinanceAndOperationsSecurityContext Build()
	{
		return (_settings is null || !_settings.HasValue)
			? BuildMock().Object
			: new Dynamics365FinanceAndOperationsSecurityContext(
			Settings.Build(),
		Logger.Build());
		;
	}

	public Mock<IDynamics365FinanceAndOperationsSecurityContext> BuildMock()
	{
		Mock<IDynamics365FinanceAndOperationsSecurityContext> security = new();
		_ = security
			.Setup(x => x.AcquireToken(It.IsAny<CancellationToken>()))
			.ReturnsAsync("token");
		return security;
	}
}