// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Tests.Core.Infrastructure.Client;

using FluentAssertions;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Configurations;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Models;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Security;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Moq;
using Moq.Protected;

using System.Net;

public class Dynamics365FinanceAndOperationsClientTest
{
	[Fact]
	public async Task Get_should_return_hello()
	{
		Mock<IOptions<Dynamics365FinanceAndOperationsClientSettings>> options = new();
		_ = options
			.Setup(x => x.Value)
			.Returns(new Dynamics365FinanceAndOperationsClientSettings
			{
				Company = "CIE",
				Instance = new Uri("https://test.dynamics.com"),
			});
		Mock<IDynamics365FinanceAndOperationsSecurityContext> security = new();
		Mock<ILogger<Dynamics365FinanceAndOperationsClient>> logger = new();
		Mock<HttpMessageHandler> handlerMock = new(MockBehavior.Strict);
		string message = "Hello world";
		string content =
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
		handlerMock
		   .Protected()
		   // Setup the PROTECTED method to mock
		   .Setup<Task<HttpResponseMessage>>(
			  "SendAsync",
			  ItExpr.IsAny<HttpRequestMessage>(),
			  ItExpr.IsAny<CancellationToken>()
		   )
		   // prepare the expected response of the mocked http call
		   .ReturnsAsync(new HttpResponseMessage()
		   {
			   StatusCode = HttpStatusCode.OK,
			   Content = new StringContent(
					content
				),
		   })
		   .Verifiable();

		HttpClient httpClient = new(handlerMock.Object)
		{
			BaseAddress = new Uri("http://test.com/"),
		};
		Mock<IHttpClientFactory> mockHttpClientFactory = new();
		_ = mockHttpClientFactory
			.Setup(_ => _.CreateClient(It.IsAny<string>()))
			.Returns(httpClient);

		Dynamics365FinanceAndOperationsClient client = new(
				mockHttpClientFactory.Object,
				security.Object,
				options.Object,
				logger.Object);
		ODataResponse<Hello> response = await client.GetSingleAsync<ODataResponse<Hello>>(
			 "hello",
				new Dictionary<string, object>
				{ { "id", "3525" } });
		_ = response.Message.Should().Be(message);
	}

	public class Hello
	{
		public string? Message { get; init; }
	}
}