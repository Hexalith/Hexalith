// <copyright file="Dynamics365FinanceAndOperationsClient.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Configurations;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Models;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Security;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Web;

/// <summary>
/// Dynamics 365 Finance client.
/// </summary>
public class Dynamics365FinanceAndOperationsClient : IDynamics365FinanceAndOperationsClient
{
	private const string _dataPath = "data";
	private readonly string _company;
	private readonly IHttpClientFactory _httpClientFactory;
	private readonly Uri _instance;
	private readonly ILogger _logger;
	private readonly IDynamics365FinanceAndOperationsSecurityContext _securityContext;
	private HttpClient? _client;

	/// <summary>
	/// Initializes a new instance of the <see cref="Dynamics365FinanceAndOperationsClient"/> class.
	/// </summary>
	/// <param name="httpClientFactory">The HTTP client factory.</param>
	/// <param name="securityContext">The Dynamics 365 security context.</param>
	/// <param name="settings">The client settings.</param>
	/// <param name="logger">The client logger.</param>
	public Dynamics365FinanceAndOperationsClient(
		IHttpClientFactory httpClientFactory,
		IDynamics365FinanceAndOperationsSecurityContext securityContext,
		IOptions<Dynamics365FinanceAndOperationsClientSettings> settings,
		ILogger<Dynamics365FinanceAndOperationsClient> logger)
	{
		if (settings == null)
		{
			throw new ArgumentNullException(nameof(settings));
		}

		Dynamics365FinanceAndOperationsClientSettings s = settings.Value;
		_httpClientFactory = httpClientFactory;
		_securityContext = securityContext;
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		if (string.IsNullOrWhiteSpace(s.Instance?.OriginalString))
		{
			throw new ArgumentException(
				$"The {nameof(s.Instance)} setting is not defined.",
				nameof(settings));
		}

		if (string.IsNullOrWhiteSpace(s.Company))
		{
			throw new ArgumentException(
				$"The {nameof(s.Company)} setting is not defined.",
				nameof(settings));
		}

		_company = s.Company;
		_instance = s.Instance;
	}

	private HttpClient Client => _client ??= _httpClientFactory.CreateClient();

	/// <inheritdoc/>
	public Task DoActionAsync(string entityName, string action, IDictionary<string, object> parameters, CancellationToken cancellationToken)
	{
		// TODO Implement action call.
		throw new NotImplementedException();
	}

	/// <inheritdoc/>
	public async Task<IEnumerable<T>> GetAsync<T>(string entityName, IDictionary<string, object> filter, CancellationToken cancellationToken)
	{
		await AddRequestHeadersAsync(cancellationToken).ConfigureAwait(false);

		Uri url = new(_instance, $"{_dataPath}/{entityName}/?$filter={HttpUtility.UrlEncode(GetQueryFilter(filter))}");
		HttpResponseMessage? response = null;
		try
		{
			response = await Client.GetAsync(url, cancellationToken).ConfigureAwait(false);
			if (response.IsSuccessStatusCode)
			{
				ODataArrayResponse<T>? content = await response
					.Content
					.ReadFromJsonAsync<ODataArrayResponse<T>>(
					options: new JsonSerializerOptions
					{
						PropertyNameCaseInsensitive = true,
					},
					cancellationToken).ConfigureAwait(false);
				if (content != null && !string.IsNullOrWhiteSpace(content.Context))
				{
					_logger.LogTrace("The method call to '{Path}' was successful.", url.AbsoluteUri);
					return content.Values
						?? Enumerable.Empty<T>();
				}

				throw new HttpRequestException(
					$"The request to '{url.AbsoluteUri}' failed to deserialize :\n{await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)}");
			}

			throw new HttpRequestException(
				$"The request to '{url.AbsoluteUri}' failed with status code '{response.StatusCode}'.");
		}
		catch
		{
			_logger.LogError(
				"The method call to '{Path}' failed. response content :\n{ResponseContent}",
				url.AbsoluteUri,
				response == null ? "No response" : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false));
			throw;
		}
	}

	/// <inheritdoc/>
	public async Task<T> GetSingleAsync<T>(string entityName, IDictionary<string, object> keys, CancellationToken cancellationToken)
	{
		await AddRequestHeadersAsync(cancellationToken).ConfigureAwait(false);
		string keyFilter = GetEntityFilter((Dictionary<string, object>)keys);
		Uri url = new(_instance, $"{_dataPath}/{entityName}({HttpUtility.UrlEncode(keyFilter)})");
		HttpResponseMessage? response = null;
		try
		{
			response = await Client.GetAsync(url, cancellationToken).ConfigureAwait(false);
			if (response.IsSuccessStatusCode)
			{
				T? content = await response
					.Content
					.ReadFromJsonAsync<T>(
					options: new JsonSerializerOptions
					{
						PropertyNameCaseInsensitive = true,
					},
					cancellationToken).ConfigureAwait(false);
				if (content == null)
				{
					throw new HttpRequestException($"Empty content response on request to '{url.AbsoluteUri}'.");
				}

				_logger.LogDebug("The method call to '{Path}' was successful.", url.AbsoluteUri);
				return content;
			}

			throw new HttpRequestException(
				$"The request to '{url.AbsoluteUri}' failed with status code '{response.StatusCode}'.");
		}
		catch (Exception ex)
		{
			string? responseContent = (response == null) ? null : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
			_logger.LogError(
				"Can't get {EntityName} with keys {Keys}. The method call to '{Path}' failed. response content :\n{ResponseContent}",
				typeof(T).Name,
				keys,
				url.AbsoluteUri,
				responseContent ?? "No response");
			throw new GetSingleRequestFailedException<T>(entityName, keys, responseContent, message: null, ex);
		}
	}

	/// <inheritdoc/>
	public async Task PatchAsync<T>(
		string entityName,
		IDictionary<string, object> key,
		T value,
		CancellationToken cancellationToken)
	{
		await AddRequestHeadersAsync(cancellationToken).ConfigureAwait(false);
		Uri url = new(_instance, $"{_dataPath}/{entityName}({HttpUtility.UrlEncode(GetEntityFilter((Dictionary<string, object>)key))})");
		HttpResponseMessage? response = null;
		try
		{
			response = await Client
				.PatchAsync(
					url,
					JsonContent
						.Create(
						value,
						mediaType: null,
						new JsonSerializerOptions
						{
							PropertyNameCaseInsensitive = false,
							PropertyNamingPolicy = null,
						}),
					cancellationToken).ConfigureAwait(false);
			if (response.IsSuccessStatusCode)
			{
				return;
			}

			throw new HttpRequestException(
				$"The patch request '{url.AbsoluteUri}' failed with status code '{response.StatusCode}'. Response:\n{await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)}");
		}
		catch
		{
			_logger.LogError(
				"The method call to '{Path}' failed. response content :\n{ResponseContent}",
				url.AbsoluteUri,
				response == null ? "No response" : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false));
			throw;
		}
	}

	/// <inheritdoc/>
	public async Task<TEntity> PostAsync<TCreate, TEntity>(string entityName, TCreate value, CancellationToken cancellationToken)
	{
		HttpResponseMessage response = await PostAsync(entityName, value, cancellationToken).ConfigureAwait(false);
		TEntity? v = await response
			.Content
			.ReadFromJsonAsync<TEntity>(
				new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true,
				},
				cancellationToken).ConfigureAwait(false);
		return v ?? throw new HttpRequestException($"Empty content response on request to '{response.RequestMessage?.RequestUri?.AbsoluteUri}'.");
	}

	/// <inheritdoc/>
	public async Task<HttpResponseMessage> PostAsync<T>(string entityName, T value, CancellationToken cancellationToken)
	{
		await AddRequestHeadersAsync(cancellationToken).ConfigureAwait(false);
		Uri url = new(_instance, $"{_dataPath}/{entityName}");
		HttpResponseMessage? response = null;
		try
		{
			response = await Client
				.PostAsJsonAsync(
					url,
					value,
					new JsonSerializerOptions
					{
						PropertyNameCaseInsensitive = false,
						PropertyNamingPolicy = null,
					},
					cancellationToken).ConfigureAwait(false);
			if (response?.IsSuccessStatusCode == true)
			{
				_logger.LogInformation("The method call to '{Post}' succeeded.", url.AbsolutePath);
				return response;
			}

			throw new HttpRequestException(
				$"The patch request '{url.AbsoluteUri}' failed with status code '{response?.StatusCode}'. Response:\n{(response == null ? null : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false))}");
		}
		catch
		{
			_logger.LogError(
				"The method call to '{Path}' failed. response content :\n{ResponseContent}",
				url.AbsoluteUri,
				response == null ? "No response" : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false));
			throw;
		}
	}

	private static string GetOdataString(object value)
	{
		return value is string s ? $"'{s}'" : Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
	}

	private async Task AddRequestHeadersAsync(CancellationToken cancellationToken)
	{
		Client.DefaultRequestHeaders.Clear();
		Client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
		Client.DefaultRequestHeaders.Add("OData-Version", "4.0");
		Client.DefaultRequestHeaders.Add("Prefer", "odata.include-annotations = *");
		Client.DefaultRequestHeaders.Accept.Add(
			new MediaTypeWithQualityHeaderValue("application/json"));
		Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
			"Bearer",
			await _securityContext.AcquireTokenAsync(cancellationToken).ConfigureAwait(false));
	}

	private string GetEntityFilter(Dictionary<string, object> keys)
	{
		StringBuilder filter = new();
		_ = filter.Append(CultureInfo.InvariantCulture, $"dataAreaId='{_company}'");
		foreach (KeyValuePair<string, object> key in keys)
		{
			_ = filter.Append(CultureInfo.InvariantCulture, $",{key.Key}={GetOdataString(key.Value)}");
		}

		return filter.ToString();
	}

	private string GetQueryFilter(IDictionary<string, object> filter)
	{
		StringBuilder query = new();
		_ = query.Append(CultureInfo.InvariantCulture, $"dataAreaId eq '{_company}'");
		foreach (KeyValuePair<string, object> key in filter)
		{
			_ = query.Append(CultureInfo.InvariantCulture, $" and {key.Key} eq {GetOdataString(key.Value)}");
		}

		return query.ToString();
	}
}