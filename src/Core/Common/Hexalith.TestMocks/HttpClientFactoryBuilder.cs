// <copyright file="HttpClientFactoryBuilder.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.TestMocks;

using System.Net;

using Moq;
using Moq.Protected;

/// <summary>
/// Helper class to build a <see cref="IHttpClientFactory" /> mock.
/// </summary>
public class HttpClientFactoryBuilder : IDisposable
{
    /// <summary>
    /// The disposed value.
    /// </summary>
    private bool _disposedValue;

    /// <summary>
    /// The HTTP client.
    /// </summary>
    private HttpClient? _httpClient;

    /// <summary>
    /// The HTTP handler.
    /// </summary>
    private HttpMessageHandler? _httpHandler;

    private HttpResponseMessage? _httpResponse;

    /// <summary>
    /// Gets the HTTP client.
    /// </summary>
    /// <value>The HTTP client.</value>
    private HttpClient HttpClient => _httpClient ??= new(HttpHandler)
    {
        BaseAddress = new Uri("http://test.hexalith.com/"),
    };

    private HttpMessageHandler HttpHandler => _httpHandler ?? new HttpClientHandler();

    /// <summary>
    /// Builds this instance.
    /// </summary>
    /// <returns>IHttpClientFactory.</returns>
    public IHttpClientFactory Build() => BuildMock().Object;

    /// <summary>
    /// Build a <see cref="Mock{IHttpClientFactory}" /> with the specified <see cref="HttpMessageHandler" />.
    /// </summary>
    /// <returns>A HTTP client factory with mocked dependencies if handler is defined, else a mock of the factory.</returns>
    /// <exception cref="InvalidOperationException">HttpMessageHandler already set.</exception>
    public Mock<IHttpClientFactory> BuildMock()
    {
        Mock<IHttpClientFactory> mockHttpClientFactory = new();
        _ = mockHttpClientFactory
            .Setup(_ => _.CreateClient(It.IsAny<string>()))
            .Returns(HttpClient);
        return mockHttpClientFactory;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Define the <see cref="HttpMessageHandler" /> to use.
    /// </summary>
    /// <param name="httpHandler">HTTP handler instance.</param>
    /// <returns>The factory builder.</returns>
    /// <exception cref="System.InvalidOperationException">HttpMessageHandler already set.</exception>
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
    /// Define a mocked <see cref="HttpMessageHandler" /> to use. The mock will return a <see cref="HttpResponseMessage" /> with the specified content.
    /// </summary>
    /// <param name="response">The content to return.</param>
    /// <returns>The factory builder.</returns>
    public HttpClientFactoryBuilder SetMockHttpMessageHandler(string response)
    {
        if (_httpHandler != null)
        {
            throw new InvalidOperationException("HttpMessageHandler already set.");
        }

        if (_httpResponse != null)
        {
            throw new InvalidOperationException("Response already set.");
        }

        Mock<HttpMessageHandler> handlerMock = new(MockBehavior.Strict);

        _httpResponse = new()
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(
                    response),
        };
        handlerMock
           .Protected()

           // Setup the PROTECTED method to mock
           .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>())

           // prepare the expected response of the mocked HTTP call
           .ReturnsAsync(_httpResponse)
           .Verifiable();
        handlerMock
           .Protected()

           // Setup the PROTECTED method to mock
           .Setup(
              "Dispose",
              ItExpr.IsAny<bool>())

           .Verifiable();
        return SetHttpMessageHandler(handlerMock.Object);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _httpResponse?.Dispose();
                _httpHandler?.Dispose();
                _httpClient?.Dispose();
            }

            _disposedValue = true;
        }
    }
}