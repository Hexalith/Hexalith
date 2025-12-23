// <copyright file="HttpClientFactoryBuilder.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.TestMocks;

using System.Net;

using NSubstitute;

using RichardSzalay.MockHttp;

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

    /// <summary>
    /// The mock HTTP message handler.
    /// </summary>
    private MockHttpMessageHandler? _mockHttpMessageHandler;

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
    public IHttpClientFactory Build()
    {
        IHttpClientFactory factory = Substitute.For<IHttpClientFactory>();
        factory.CreateClient(Arg.Any<string>()).Returns(HttpClient);
        return factory;
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

        if (_mockHttpMessageHandler != null)
        {
            throw new InvalidOperationException("Response already set.");
        }

        _mockHttpMessageHandler = new MockHttpMessageHandler();
        _mockHttpMessageHandler.When("*").Respond(HttpStatusCode.OK, "text/plain", response);

        return SetHttpMessageHandler(_mockHttpMessageHandler);
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
                _mockHttpMessageHandler?.Dispose();
                _httpHandler?.Dispose();
                _httpClient?.Dispose();
            }

            _disposedValue = true;
        }
    }
}
