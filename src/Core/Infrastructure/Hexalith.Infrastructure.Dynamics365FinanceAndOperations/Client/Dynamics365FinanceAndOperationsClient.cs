// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Configurations;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Models;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Security;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

/// <summary>
/// Dynamics 365 Finance client.
/// </summary>
public class Dynamics365FinanceAndOperationsClient : IDynamics365FinanceAndOperationsClient
{
	private const string _dataPath = "data";
	private const string _domain = "Microsoft.Dynamics.DataEntities";
	private readonly string _company;
	private readonly IHttpClientFactory _httpClientFactory;
	private readonly Uri _instance;
	private readonly ILogger _logger;
	private readonly IDynamics365FinanceAndOperationsSecurityContext _securityContext;
	private readonly Dynamics365FinanceAndOperationsClientSettings _settings;
	private HttpClient? _client;

	public Dynamics365FinanceAndOperationsClient(
		IHttpClientFactory httpClientFactory,
		IDynamics365FinanceAndOperationsSecurityContext securityContext,
		IOptions<Dynamics365FinanceAndOperationsClientSettings> settings,
		ILogger<Dynamics365FinanceAndOperationsClient> logger)
	{
		_settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
		_httpClientFactory = httpClientFactory;
		_securityContext = securityContext;
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		if (string.IsNullOrWhiteSpace(_settings.Instance?.OriginalString))
		{
			throw new ArgumentException($"The {nameof(_settings.Instance)} setting is not defined.",
										nameof(settings));
		}
		if (string.IsNullOrWhiteSpace(_settings.Company))
		{
			throw new ArgumentException($"The {nameof(_settings.Company)} setting is not defined.",
										nameof(settings));
		}
		_company = _settings.Company;
		_instance = _settings.Instance;
	}

	private HttpClient Client => _client ??= _httpClientFactory.CreateClient();

	public Task DoActionAsync(string headerEntityName, string action, Dictionary<string, object> dictionary, CancellationToken cancellationToken) => throw new NotImplementedException();

	public async Task<IEnumerable<T>> GetAsync<T>(string entityName, Dictionary<string, object> filter, CancellationToken cancellationToken = default)
	{
		await AddRequestHeaders(cancellationToken);

		Uri url = new(_instance, $"{_dataPath}/{entityName}/?$filter={HttpUtility.UrlEncode(GetQueryFilter(filter))}");
		HttpResponseMessage? response = null;
		try
		{
			response = await Client.GetAsync(url, cancellationToken);
			if (response.IsSuccessStatusCode)
			{
				ODataArrayResponse<T>? content = await response
					.Content
					.ReadFromJsonAsync<ODataArrayResponse<T>>
					(options: new JsonSerializerOptions()
					{
						PropertyNameCaseInsensitive = true
					},
					cancellationToken);
				if (content != null && !string.IsNullOrWhiteSpace(content.Context))
				{
					_logger.LogTrace("The method call to '{Path}' was succesfull.", url.AbsoluteUri);
					return content.Values
						?? Enumerable.Empty<T>();
				}
				throw new HttpRequestException(
					$"The request to '{url.AbsoluteUri}' failed to deserialize :\n{await response.Content.ReadAsStringAsync(cancellationToken)}");
			}
			throw new HttpRequestException(
				$"The request to '{url.AbsoluteUri}' failed with status code '{response.StatusCode}'.");
		}
		catch
		{
			_logger.LogError("The method call to '{Path}' failed. response content :\n{ResponseContent}",
				url.AbsoluteUri,
				 response == null
				 ? "No response"
				 : await response.Content.ReadAsStringAsync(cancellationToken));
			throw;
		}
	}

	public async Task<T> GetSingleAsync<T>(string entityName, Dictionary<string, object> keyValues, CancellationToken cancellationToken = default)
	{
		await AddRequestHeaders(cancellationToken);
		string keys = GetEntityFilter(keyValues);
		Uri url = new(_instance, $"{_dataPath}/{entityName}({HttpUtility.UrlEncode(keys)})");
		HttpResponseMessage? response = null;
		try
		{
			response = await Client.GetAsync(url, cancellationToken);
			if (response.IsSuccessStatusCode)
			{
				ODataResponse<T>? content = await response
					.Content
					.ReadFromJsonAsync<ODataResponse<T>>
					(options: new JsonSerializerOptions()
					{
						PropertyNameCaseInsensitive = true
					},
					cancellationToken);
				if (content == null)
				{
					throw new HttpRequestException($"Empty content response on request to '{url.AbsoluteUri}'.");
				}
				if (content.Value == null)
				{
					throw new HttpRequestException($"Empty value response on request to '{url.AbsoluteUri}'.\nMessage:{content.Message}");
				}
				_logger.LogDebug("The method call to '{Path}' was succesfull.", url.AbsoluteUri);
				return content.Value;
			}
			throw new HttpRequestException(
				$"The request to '{url.AbsoluteUri}' failed with status code '{response.StatusCode}'.");
		}
		catch (Exception ex)
		{
			_logger.LogError("Can't get {EntityName} with keys {Keys}. The method call to '{Path}' failed. response content :\n{ResponseContent}",
				typeof(T).Name,
				keys,
				url.AbsoluteUri,
				 response == null
				 ? "No response"
				 : await response.Content.ReadAsStringAsync(cancellationToken));
			throw new Exception($"Failed to retreive {typeof(T).Name} with keys {keys}.", ex);
		}
	}

	public async Task PatchAsync<T>(
		string entityName,
		Dictionary<string, object> keyValues,
		T patchValues,
		CancellationToken cancellationToken)
	{
		await AddRequestHeaders(cancellationToken);
		Uri url = new(_instance, $"{_dataPath}/{entityName}({HttpUtility.UrlEncode(GetEntityFilter(keyValues))})");
		HttpResponseMessage? response = null;
		try
		{
			response = await Client
				.PatchAsync(
					url,
					JsonContent
						.Create(
						patchValues,
						null,
						new JsonSerializerOptions()
						{
							PropertyNameCaseInsensitive = false,
							PropertyNamingPolicy = null
						}
					),
					cancellationToken); ;
			if (response.IsSuccessStatusCode)
			{
				return;
			}
			throw new HttpRequestException(
				$"The patch request '{url.AbsoluteUri}' failed with status code '{response.StatusCode}'. Response:\n{await response.Content.ReadAsStringAsync(cancellationToken)}");
		}
		catch
		{
			_logger.LogError("The method call to '{Path}' failed. response content :\n{ResponseContent}",
				url.AbsoluteUri,
				 response == null
				 ? "No response"
				 : await response.Content.ReadAsStringAsync(cancellationToken));
			throw;
		}
	}

	public async Task<R> PostAsync<T, R>(string entityName, T value, CancellationToken cancellationToken = default)
	{
		HttpResponseMessage response = await PostAsync(entityName, value, cancellationToken);
		R? v = await response
			.Content
			.ReadFromJsonAsync<R>(new JsonSerializerOptions()
			{
				PropertyNameCaseInsensitive = true
			},
			cancellationToken);
		return v ?? throw new HttpRequestException($"Empty content resonse on request to '{response.RequestMessage?.RequestUri?.AbsoluteUri}'.");
	}

	public async Task<HttpResponseMessage> PostAsync<T>(string entityName, T value, CancellationToken cancellationToken = default)
	{
		await AddRequestHeaders(cancellationToken);
		Uri url = new(_instance, $"{_dataPath}/{entityName}");
		HttpResponseMessage? response = null;
		try
		{
			response = await Client
				.PostAsJsonAsync(
					url,
					value,
					new JsonSerializerOptions()
					{
						PropertyNameCaseInsensitive = false,
						PropertyNamingPolicy = null
					},
					cancellationToken);
			if (response?.IsSuccessStatusCode == true)
			{
				_logger.LogInformation("The method call to '{Post}' succeeded.", url.AbsolutePath);
				return response;
			}
			throw new HttpRequestException(
				$"The patch request '{url.AbsoluteUri}' failed with status code '{response?.StatusCode}'. Response:\n{(response == null ? null : await response.Content.ReadAsStringAsync(cancellationToken))}");
		}
		catch
		{
			_logger.LogError("The method call to '{Path}' failed. response content :\n{ResponseContent}",
				url.AbsoluteUri,
				 response == null
				 ? "No response"
				 : await response.Content.ReadAsStringAsync(cancellationToken));
			throw;
		}
	}

	private async Task AddRequestHeaders(CancellationToken cancellationToken = default)
	{
		Client.DefaultRequestHeaders.Clear();
		Client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
		Client.DefaultRequestHeaders.Add("OData-Version", "4.0");
		Client.DefaultRequestHeaders.Add("Prefer", "odata.include-annotations = *");
		Client.DefaultRequestHeaders.Accept.Add(
			new MediaTypeWithQualityHeaderValue("application/json")
		);
		Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
			("Bearer",
			await _securityContext.AcquireToken(cancellationToken)
		);
	}

	private string GetEntityFilter(Dictionary<string, object> keys)
	{
		StringBuilder filter = new();
		_ = filter.Append($"dataAreaId='{_company}'");
		foreach (KeyValuePair<string, object> key in keys)
		{
			string value = key.Value switch
			{
				string s => $"'{s}'",
				_ => key.Value.ToString() ?? string.Empty
			};
			_ = filter.Append($",{key.Key}={value}");
		}
		return filter.ToString();
	}

	private string GetQueryFilter(Dictionary<string, object> filter)
	{
		StringBuilder query = new();
		_ = query.Append($"dataAreaId eq '{_company}'");
		foreach (KeyValuePair<string, object> key in filter)
		{
			string value = key.Value switch
			{
				string s => $"'{s}'",
				_ => key.Value.ToString() ?? string.Empty
			};
			_ = query.Append($" and {key.Key} eq {value}");
		}
		return query.ToString();
	}
}