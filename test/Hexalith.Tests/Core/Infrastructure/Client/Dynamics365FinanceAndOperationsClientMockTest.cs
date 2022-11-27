// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Tests.Core.Infrastructure.Client;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Configurations;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.TestMocks;

public class Dynamics365FinanceAndOperationsClientMockTest
{
	[Fact]
	public void Build_client_with_mocked_response_should_succeed()
	{
		Dynamics365FinanceAndOperationsClientBuilder builder = new Dynamics365FinanceAndOperationsClientBuilder()
			.WithSettingsValue(new Dynamics365FinanceAndOperationsClientSettings { Company = "CIE", Instance = new Uri("https://test.dynamics.com") });
		_ = builder.HttpClientfactory.SetMockHttpMessageHandler("dummy response");
		_ = builder.Build();
	}
}