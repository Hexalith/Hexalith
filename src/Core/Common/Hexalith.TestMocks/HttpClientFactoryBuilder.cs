// Fiveforty S.A. Paris France (2022)
namespace Hexalith.TestMocks;

using Moq;
using Moq.Protected;

using System.Net;
using System.Net.Http;

public class HttpClientFactoryBuilder
{
	private HttpMessageHandler? _httpHandler;

	public IHttpClientFactory Build() => BuildMock().Object;

	public Mock<IHttpClientFactory> BuildMock()
	{
		HttpClient httpClient = new(_httpHandler ?? throw new InvalidOperationException("No HttpMessageHandler defined."))
		{
			BaseAddress = new Uri("http://test.hexalith.com/"),
		};
		Mock<IHttpClientFactory> mockHttpClientFactory = new();
		_ = mockHttpClientFactory
			.Setup(_ => _.CreateClient(It.IsAny<string>()))
			.Returns(httpClient);
		return mockHttpClientFactory;
	}

	public HttpClientFactoryBuilder SetHttpMessageHandler(HttpMessageHandler httpHandler)
	{
		if (_httpHandler != null)
		{
			throw new InvalidOperationException("HttpMessageHandler already set.");
		}
		_httpHandler = httpHandler;
		return this;
	}

	public HttpClientFactoryBuilder SetMockHttpMessageHandler(string response)
	{
		Mock<HttpMessageHandler> handlerMock = new(MockBehavior.Strict);
		handlerMock
		   .Protected()
		   // Setup the PROTECTED method to mock
		   .Setup<Task<HttpResponseMessage>>(
			  "SendAsync",
			  ItExpr.IsAny<HttpRequestMessage>(),
			  ItExpr.IsAny<CancellationToken>()
		   )
		   // prepare the expected response of the mocked http call
		   .ReturnsAsync(new HttpResponseMessage
		   {
			   StatusCode = HttpStatusCode.OK,
			   Content = new StringContent(
					response
				),
		   })
		   .Verifiable();
		return SetHttpMessageHandler(handlerMock.Object);
	}
}