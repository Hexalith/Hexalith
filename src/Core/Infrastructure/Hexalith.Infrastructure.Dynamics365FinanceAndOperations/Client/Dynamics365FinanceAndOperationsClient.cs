// <copyright file="Dynamics365FinanceAndOperationsClient.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;

using Ardalis.GuardClauses;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Configurations;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Models;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Security;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

/// <summary>
/// Client for Dynamics 365 for finance and operations.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public class Dynamics365FinanceAndOperationsClient<TEntity> : IDynamics365FinanceAndOperationsClient<TEntity>
    where TEntity : class, IODataElement
{
    private const string _crossCompanyQuery = "cross-company=true";
    private const string _dataPath = "data";
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly Uri _instance;
    private readonly ILogger<Dynamics365FinanceAndOperationsClient<TEntity>> _logger;
    private readonly IDynamics365FinanceAndOperationsSecurityContext _securityContext;
    private HttpClient? _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceAndOperationsClient{TEntity}"/> class.
    /// </summary>
    /// <param name="httpClientFactory">The HTTP client factory.</param>
    /// <param name="securityContext">The Dynamics 365 security context.</param>
    /// <param name="settings">The client settings.</param>
    /// <param name="logger">The client logger.</param>
    /// <exception cref="ArgumentException">Setting not defined.</exception>
    public Dynamics365FinanceAndOperationsClient(
        IHttpClientFactory httpClientFactory,
        IDynamics365FinanceAndOperationsSecurityContext securityContext,
        IOptions<Dynamics365FinanceAndOperationsClientSettings> settings,
        ILogger<Dynamics365FinanceAndOperationsClient<TEntity>> logger)
    {
        Dynamics365FinanceAndOperationsClientSettings s = Guard.Against.Null(settings).Value;

        _httpClientFactory = Guard.Against.Null(httpClientFactory);
        _securityContext = Guard.Against.Null(securityContext);
        _logger = Guard.Against.Null(logger);
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

        DefaultCompany = s.Company;
        _instance = s.Instance;
    }

    /// <inheritdoc/>
    public string DefaultCompany { get; }

    /// <summary>
    /// Gets the json options.
    /// </summary>
    /// <value>The json options.</value>
    protected JsonSerializerOptions JsonOptions => new()
    {
        PropertyNameCaseInsensitive = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    private HttpClient Client => _client ??= _httpClientFactory.CreateClient();

    /// <inheritdoc/>
    public Task DoActionAsync(string action, IDictionary<string, object> parameters, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    /// <inheritdoc/>
    public Task<IEnumerable<TEntity>> GetAsync(IDictionary<string, object> filter, CancellationToken cancellationToken)
    {
        return GetAsync(DefaultCompany, filter, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TEntity>> GetAsync(string company, IDictionary<string, object> filter, CancellationToken cancellationToken)
    {
        string crossCompany = string.Equals(DefaultCompany, company, StringComparison.InvariantCultureIgnoreCase) ? string.Empty : _crossCompanyQuery + "&";
        await AddRequestHeadersAsync(cancellationToken).ConfigureAwait(false);
        Uri url = new(_instance, $"{_dataPath}/{TEntity.EntityName()}/?{crossCompany}$filter={HttpUtility.UrlEncode(GetQueryFilter(company, filter))}");
        HttpResponseMessage? response = null;
        try
        {
            response = await Client.GetAsync(url, cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"The request to '{url.AbsoluteUri}' failed with status code '{response.StatusCode}'.");
            }

            ODataArrayResponse<TEntity>? content = await response
                    .Content
                    .ReadFromJsonAsync<ODataArrayResponse<TEntity>>(
                    options: JsonOptions,
                    cancellationToken)
                    .ConfigureAwait(false);
            if (content != null && !string.IsNullOrWhiteSpace(content.Context))
            {
                _logger.LogTrace("The method call to '{Path}' was successful.", url.AbsoluteUri);
                return content.Values ?? Enumerable.Empty<TEntity>();
            }

            throw new HttpRequestException(
                    $"The request to '{url.AbsoluteUri}' failed to deserialize :\n{await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)}");
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
    public Task<TEntity> GetSingleAsync(IDictionary<string, object> keys, CancellationToken cancellationToken)
    {
        return GetSingleAsync(DefaultCompany, keys, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TEntity> GetSingleAsync(string company, IDictionary<string, object> keys, CancellationToken cancellationToken)
    {
        string crossCompany = string.Equals(DefaultCompany, company, StringComparison.InvariantCultureIgnoreCase) ? string.Empty : "?" + _crossCompanyQuery;
        await AddRequestHeadersAsync(cancellationToken).ConfigureAwait(false);
        string keyFilter = GetEntityFilter(company, keys);
        Uri url = new(_instance, $"{_dataPath}/{TEntity.EntityName()}({HttpUtility.UrlEncode(keyFilter)}){crossCompany}");
        HttpResponseMessage? response = null;
        try
        {
            response = await Client.GetAsync(url, cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"The request to '{url.AbsoluteUri}' failed with status code '{response.StatusCode}'.");
            }

            TEntity? content = await response
                    .Content
                    .ReadFromJsonAsync<TEntity>(
                    options: JsonOptions,
                    cancellationToken).ConfigureAwait(false);
            if (content is null)
            {
                throw new HttpRequestException($"Empty content response on request to '{url.AbsoluteUri}'.");
            }

            _logger.LogDebug("The method call to '{Path}' was successful.", url.AbsoluteUri);
            return content;
        }
        catch (Exception ex)
        {
            string? responseContent = (response == null) ? null : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogError(
                "Can't get {EntityName} with keys {Keys}. The method call to '{Path}' failed. response content :\n{ResponseContent}",
                typeof(TEntity).Name,
                keys,
                url.AbsoluteUri,
                responseContent ?? "No response");
            throw new GetSingleRequestFailedException<TEntity>(TEntity.EntityName(), keys, responseContent, message: null, ex);
        }
    }

    /// <inheritdoc/>
    public Task PatchAsync<TUpdate>(
        IDictionary<string, object> key,
        TUpdate value,
        CancellationToken cancellationToken)
    {
        return PatchAsync(DefaultCompany, key, value, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task PatchAsync<TUpdate>(
        string company,
        IDictionary<string, object> key,
        TUpdate value,
        CancellationToken cancellationToken)
    {
        _ = await SendPatchAsync(key, value, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<TEntity> PostAsync<TCreate>(TCreate value, CancellationToken cancellationToken)
    {
        TEntity result = await PostAsync(DefaultCompany, value, cancellationToken)
            .ConfigureAwait(false);
        return result;
    }

    /// <inheritdoc/>
    public async Task<TEntity> PostAsync<TCreate>(string company, TCreate value, CancellationToken cancellationToken)
    {
        HttpResponseMessage? response = null;
        try
        {
            response = await SendPostAsync(company, value, cancellationToken).ConfigureAwait(false);
            TEntity? v = await response
                .Content
                .ReadFromJsonAsync<TEntity>(
                    options: JsonOptions,
                    cancellationToken).ConfigureAwait(false);
            return v ?? throw new SerializationException($"Empty content response on request to '{response.RequestMessage?.RequestUri?.AbsoluteUri}'.");
        }
        catch (Exception e)
        {
            string? responseContent = response == null ? null : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            throw new Dynamics365FinancePostException<TEntity, TCreate>(company, value, responseContent, e);
        }
    }

    /// <inheritdoc/>
    public Task<HttpResponseMessage> SendPatchAsync<TUpdate>(
        IDictionary<string, object> key,
        TUpdate value,
        CancellationToken cancellationToken)
    {
        return SendPatchAsync(DefaultCompany, key, value, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<HttpResponseMessage> SendPatchAsync<TUpdate>(
        string company,
        IDictionary<string, object> key,
        TUpdate value,
        CancellationToken cancellationToken)
    {
        string crossCompany = string.Equals(DefaultCompany, company, StringComparison.InvariantCultureIgnoreCase) ? string.Empty : "?" + _crossCompanyQuery;
        await AddRequestHeadersAsync(cancellationToken).ConfigureAwait(false);
        Uri url = new(_instance, $"{_dataPath}/{TEntity.EntityName()}({HttpUtility.UrlEncode(GetEntityFilter(company, key))}){crossCompany}");
        HttpResponseMessage? response = null;
        try
        {
            response = await Client
                .PatchAsJsonAsync(
                    url,
                    value,
                    options: JsonOptions,
                    cancellationToken)
                .ConfigureAwait(false);
            if (response == null)
            {
                throw new HttpRequestException(
                    $"The patch request '{url.AbsoluteUri}' failed. The HTTP response is null.");
            }

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("The patch method call to '{Patch}' succeeded.", url.AbsolutePath);
                return response;
            }

            throw new HttpRequestException($"The patch request failed with status code '{response.StatusCode}'.");
        }
        catch (Exception e)
        {
            string? responseContent = response == null ? null : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            throw new PatchRequestFailedException(
                TEntity.EntityName(),
                url,
                value,
                responseContent,
                message: null,
                e);
        }
    }

    /// <inheritdoc/>
    public Task<HttpResponseMessage> SendPostAsync<TCreate>(TCreate value, CancellationToken cancellationToken)
    {
        return SendPostAsync(DefaultCompany, value, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<HttpResponseMessage> SendPostAsync<TCreate>(string company, TCreate value, CancellationToken cancellationToken)
    {
        string crossCompany = string.Equals(DefaultCompany, company, StringComparison.InvariantCultureIgnoreCase) ? string.Empty : "/?" + _crossCompanyQuery;
        await AddRequestHeadersAsync(cancellationToken).ConfigureAwait(false);
        Uri url = new(_instance, $"{_dataPath}/{TEntity.EntityName()}{crossCompany}");
        HttpResponseMessage? response = null;
        try
        {
            response = await Client
                .PostAsJsonAsync(
                    url,
                    value,
                    options: JsonOptions,
                    cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("The method call to '{Post}' succeeded.", url.AbsolutePath);
                return response;
            }

            throw new HttpRequestException($"The post request failed with status code '{response?.StatusCode}'.");
        }
        catch (Exception e)
        {
            string content = "No response";
            if (response != null)
            {
                content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            }

            _logger.LogError(
                e,
                "The method call to '{Path}' failed. response content :\n{ResponseContent}",
                url.AbsoluteUri,
                content);
            throw new HttpRequestException($"The method call to '{url.AbsoluteUri}' failed. response content :\n{content}", e);
        }
    }

    private static string GetEntityFilter(string company, IDictionary<string, object> keys)
    {
        StringBuilder filter = new();
        _ = filter.Append(CultureInfo.InvariantCulture, $"dataAreaId='{company}'");
        foreach (KeyValuePair<string, object> key in keys)
        {
            _ = filter.Append(CultureInfo.InvariantCulture, $",{key.Key}={GetOdataString(key.Value)}");
        }

        return filter.ToString();
    }

    private static string GetOdataString(object value)
    {
        return value is string s ? $"'{s}'" : Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
    }

    private static string GetQueryFilter(string company, IDictionary<string, object> filter)
    {
        StringBuilder query = new();
        _ = query.Append(CultureInfo.InvariantCulture, $"dataAreaId eq '{company}'");
        foreach (KeyValuePair<string, object> key in filter)
        {
            _ = query.Append(CultureInfo.InvariantCulture, $" and {key.Key} eq {GetOdataString(key.Value)}");
        }

        return query.ToString();
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
}
