// <copyright file="HttpClientFactoryBuilder.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.TestMocks;

using Moq;
using Moq.Protected;

using System.Net;

/// <summary>
/// Helper class to build a <see cref="IHttpClientFactory"/> mock.
/// </summary>
public class HttpClientFactoryBuilder
{
	private HttpMessageHandler? _httpHandler;

	public IHttpClientFactory Build()
	{
		return BuildMock().Object;
	}

	/// <summary>
	/// Build a <see cref="Mock{IHttpClientFactory}"/> with the specified <see cref="HttpMessageHandler"/>.
	/// </summary>
	/// <returns>A HTTP client factory with mocked dependencies if handler is defined, else a mock of the factory.</returns>
	/// <exception cref="InvalidOperationException">HttpMessageHandler already set.</exception>
	public Mock<IHttpClientFactory> BuildMock()
	{
		HttpClient httpClient = new(_httpHandler ?? new HttpClientHandler())
		{
			BaseAddress = new Uri("http://test.hexalith.com/"),
		};
		Mock<IHttpClientFactory> mockHttpClientFactory = new();
		_ = mockHttpClientFactory
			.Setup(_ => _.CreateClient(It.IsAny<string>()))
			.Returns(httpClient);
		return mockHttpClientFactory;
	}

	/// <summary>
	/// Define the <see cref="HttpMessageHandler"/> to use.
	/// </summary>
	/// <param name="httpHandler">HTTP handler instance.</param>
	/// <returns>The factory builder.</returns>
	/// <exception cref="InvalidOperationException">HttpMessageHandler already set.</exception>
	public HttpClientFactoryBuilder SetHttpMessageHandler(HttpMessageHandler httpHandler)
	{
		if (_httpHandler != null)
		{
			throw new InvalidOperationException("HttpMessageHandler already set.");
		}

		_httpHandler = httpHandler;
		return this;
	}

	/// <summary>
	/// Define a mocked <see cref="HttpMessageHandler"/> to use. The mock will return a <see cref="HttpResponseMessage"/> with the specified content.
	/// </summary>
	/// <param name="response">The content to return.</param>
	/// <returns>The factory builder.</returns>
	public HttpClientFactoryBuilder SetMockHttpMessageHandler(string response)
	{
		Mock<HttpMessageHandler> handlerMock = new(MockBehavior.Strict);
		handlerMock
		   .Protected()

		   // Setup the PROTECTED method to mock
		   .Setup<Task<HttpResponseMessage>>(
			  "SendAsync",
			  ItExpr.IsAny<HttpRequestMessage>(),
			  ItExpr.IsAny<CancellationToken>())

		   // prepare the expected response of the mocked HTTP call
		   .ReturnsAsync(new HttpResponseMessage
		   {
			   StatusCode = HttpStatusCode.OK,
			   Content = new StringContent(
					response),
		   })
		   .Verifiable();
		return SetHttpMessageHandler(handlerMock.Object);
	}
}