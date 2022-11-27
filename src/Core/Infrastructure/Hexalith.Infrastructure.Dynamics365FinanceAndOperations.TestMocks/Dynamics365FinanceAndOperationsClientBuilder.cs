// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.TestMocks;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Configurations;
using Hexalith.TestMocks;

using Moq;

public class Dynamics365FinanceAndOperationsClientBuilder
{
	private HttpClientFactoryBuilder? _httpClientfactory;
	private LoggerBuilder<Dynamics365FinanceAndOperationsClient>? _logger = null;
	private Dynamics365FinanceAndOperationsSecurityContextBuilder? _securityContext;
	private OptionsBuilder<Dynamics365FinanceAndOperationsClientSettings>? _settings = null;

	public HttpClientFactoryBuilder HttpClientfactory => _httpClientfactory ??= new HttpClientFactoryBuilder();

	public LoggerBuilder<Dynamics365FinanceAndOperationsClient> Logger
			=> _logger ??= new LoggerBuilder<Dynamics365FinanceAndOperationsClient>();

	public Dynamics365FinanceAndOperationsSecurityContextBuilder SecurityContext
		=> _securityContext ??= new Dynamics365FinanceAndOperationsSecurityContextBuilder();

	public OptionsBuilder<Dynamics365FinanceAndOperationsClientSettings> Settings
				=> _settings ??= new OptionsBuilder<Dynamics365FinanceAndOperationsClientSettings>();

	public static Mock<IDynamics365FinanceAndOperationsClient> BuildMock()
	{
		Mock<IDynamics365FinanceAndOperationsClient> client = new();
		return client;
	}

	public IDynamics365FinanceAndOperationsClient Build()
	{
		return _settings is null || !_settings.HasValue
			? BuildMock().Object
			: new Dynamics365FinanceAndOperationsClient(
				HttpClientfactory.Build(),
				SecurityContext.Build(),
				Settings.Build(),
				Logger.Build());
	}

	public Dynamics365FinanceAndOperationsClientBuilder WithSettingsValue(Dynamics365FinanceAndOperationsClientSettings settings)
	{
		_ = Settings.WithValue(settings);
		_ = SecurityContext.Settings.WithValue(settings);
		return this;
	}

	public Dynamics365FinanceAndOperationsClientBuilder WithValueFromConfiguration<TProgram>() where TProgram : class
	{
		_ = Settings.WithValueFromConfiguration<TProgram>();
		_ = SecurityContext.Settings.WithValueFromConfiguration<TProgram>();
		return this;
	}
}