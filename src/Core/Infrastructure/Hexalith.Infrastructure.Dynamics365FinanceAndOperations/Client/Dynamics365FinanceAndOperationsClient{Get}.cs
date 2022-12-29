// <copyright file="Dynamics365FinanceAndOperationsClient{Get}.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;

using Ardalis.GuardClauses;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Models;

using Microsoft.Extensions.Logging;

using System.Net.Http.Json;
using System.Text.Json;
using System.Web;

/// <summary>
/// Client for Dynamics 365 for finance and operations.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public partial class Dynamics365FinanceAndOperationsClient<TEntity> : IDynamics365FinanceAndOperationsClient<TEntity>
    where TEntity : class, IODataElement
{
    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(IPerCompanyPrimaryKey key, CancellationToken cancellationToken)
    {
        IDictionary<string, object?> dict = ToDictionary(key);
        _ = Guard.Against.NegativeOrZero(
            dict.Count - 1,
            message: "The key must have at least one property other than the DataAreaId.");
        string dataAreaId = string.IsNullOrWhiteSpace(key.DataAreaId) ? DefaultCompany : key.DataAreaId;
        _ = dict.Remove(nameof(IPerCompanyPrimaryKey.DataAreaId));
        IEnumerable<TEntity> result = await GetAsync(dataAreaId, dict, cancellationToken);
        int count = result.Count();
        return count == 1 || (count == 0 ? false : throw new InvalidOperationException($"The key {JsonSerializer.Serialize(key)} is not unique."));
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(ICommonPrimaryKey key, CancellationToken cancellationToken)
    {
        IDictionary<string, object?> dict = ToDictionary(key);
        _ = Guard.Against.NegativeOrZero(
            dict.Count,
            message: "The key must have at least one property other than the DataAreaId.");
        IEnumerable<TEntity> result = await GetAsync(DefaultCompany, dict, cancellationToken);
        int count = result.Count();
        return count == 1 || (count == 0 ? false : throw new InvalidOperationException($"The key {JsonSerializer.Serialize(key)} is not unique."));
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
    public Task<TEntity> GetSingleAsync(IPerCompanyPrimaryKey key, CancellationToken cancellationToken)
    {
        IDictionary<string, object?> dict = ToDictionary(key);
        _ = Guard.Against.NegativeOrZero(
            dict.Count - 1,
            message: "The key must have at least one property other than the DataAreaId.");
        string dataAreaId = string.IsNullOrWhiteSpace(key.DataAreaId) ? DefaultCompany : key.DataAreaId;
        _ = dict.Remove(nameof(IPerCompanyPrimaryKey.DataAreaId));
        return GetSingleAsync(dataAreaId, dict, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<TEntity> GetSingleAsync(ICommonPrimaryKey key, CancellationToken cancellationToken)
    {
        IDictionary<string, object?> dict = ToDictionary(key);
        _ = Guard.Against.NegativeOrZero(
            dict.Count,
            message: "The key must have at least one property other than the DataAreaId.");
        return GetSingleAsync(DefaultCompany, dict, cancellationToken);
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
}
