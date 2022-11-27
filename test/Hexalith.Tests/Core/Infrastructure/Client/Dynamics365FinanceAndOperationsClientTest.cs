// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Tests.Core.Infrastructure.Client;

using FluentAssertions;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Configurations;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.TestMocks;

public class Dynamics365FinanceAndOperationsClientTest
{
	[Fact]
	public async Task Get_should_return_hello()
	{
		Dynamics365FinanceAndOperationsClientSettings settings = new()
		{
			Company = "CIE",
			Instance = new Uri("https://test.dynamics.com"),
		};
		const string message = "Hello world";
		string response =
			$$"""
			{
				"@odata.context":"hello context",
				"message":"this is a message",
				"value":
				{
					"Message":"{{message}}"
				}
			}
			""";
		Dynamics365FinanceAndOperationsClientBuilder builder = new();
		_ = builder.Settings.WithValue(settings);
		_ = builder.HttpClientfactory.SetMockHttpMessageHandler(response);
		IDynamics365FinanceAndOperationsClient client = builder.Build();
		Hello result = await client.GetSingleAsync<Hello>(
			"hello",
			new Dictionary<string, object>
				{ { "id", "3525" } }, CancellationToken.None);
		_ = result.Message.Should().Be(message);
	}

	public class Hello
	{
		public string? Message { get; init; }
	}
}