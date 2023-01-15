// <copyright file="Dynamics365FinanceAndOperationsClient{Get}.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;

using Hexalith.Application.Abstractions.Exceptions;
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
    public async Task<int> CountAsync(IPerCompanyFilter filter, CancellationToken cancellationToken)
    {
        (string dataAreaId, IDictionary<string, object?> dict) = FilterToDictionary(filter);
        IEnumerable<TEntity> result = await GetAsync(dataAreaId, dict, cancellationToken);
        return result.Count();
    }

    /// <inheritdoc/>
    public async Task<int> CountAsync(ICommonFilter filter, CancellationToken cancellationToken)
    {
        IEnumerable<TEntity> result = await GetAsync(DefaultCompany, FilterToDictionary(filter), cancellationToken);
        return result.Count();
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(IPerCompanyPrimaryKey key, CancellationToken cancellationToken)
    {
        (string dataAreaId, IDictionary<string, object?> dict) = KeyToDictionary(key);
        IEnumerable<TEntity> result = await GetAsync(dataAreaId, dict, cancellationToken);
        int count = result.Count();
        return count == 1 || (count == 0 ? false : throw new InvalidOperationException($"The key {JsonSerializer.Serialize(key)} is not unique."));
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(ICommonPrimaryKey key, CancellationToken cancellationToken)
    {
        IEnumerable<TEntity> result = await GetAsync(DefaultCompany, KeyToDictionary(key), cancellationToken);
        int count = result.Count();
        return count == 1 || (count == 0 ? false : throw new InvalidOperationException($"The key {JsonSerializer.Serialize(key)} is not unique."));
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(IPerCompanyFilter filter, CancellationToken cancellationToken)
    {
        return await CountAsync(filter, cancellationToken) > 0;
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(ICommonFilter filter, CancellationToken cancellationToken)
    {
        return await CountAsync(filter, cancellationToken) > 0;
    }

    /// <inheritdoc/>
    public Task<IEnumerable<TEntity>> GetAsync(IDictionary<string, object?> filter, CancellationToken cancellationToken)
    {
        return GetAsync(DefaultCompany, filter, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TEntity>> GetAsync(string company, IDictionary<string, object?> filter, CancellationToken cancellationToken)
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
                Logger.LogTrace("The method call to '{Path}' was successful.", url.AbsoluteUri);
                return content.Values ?? Enumerable.Empty<TEntity>();
            }

            throw new HttpRequestException(
                    $"The request to '{url.AbsoluteUri}' failed to deserialize :\n{await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)}");
        }
        catch
        {
            Logger.LogError(
                "The method call to '{Path}' failed. response content :\n{ResponseContent}",
                url.AbsoluteUri,
                response == null ? "No response" : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false));
            throw;
        }
    }

    /// <inheritdoc/>
    public Task<IEnumerable<TEntity>> GetAsync(IPerCompanyFilter filter, CancellationToken cancellationToken)
    {
        (string dataAreaId, IDictionary<string, object?> dict) = FilterToDictionary(filter);
        return GetAsync(dataAreaId, dict, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<IEnumerable<TEntity>> GetAsync(ICommonFilter filter, CancellationToken cancellationToken)
    {
        return GetAsync(DefaultCompany, FilterToDictionary(filter), cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TEntity> GetSingleAsync(IPerCompanyFilter key, CancellationToken cancellationToken)
    {
        IEnumerable<TEntity> result = await GetAsync(key, cancellationToken);
        return result.Count() switch
        {
            0 => throw new EntityNotFoundException<TEntity>(key),
            1 => result.First(),
            _ => throw new DuplicateEntityFoundException<TEntity>(key),
        };
    }

    /// <inheritdoc/>
    public async Task<TEntity> GetSingleAsync(ICommonFilter key, CancellationToken cancellationToken)
    {
        IEnumerable<TEntity> result = await GetAsync(key, cancellationToken);
        return result.Count() switch
        {
            0 => throw new EntityNotFoundException<TEntity>(key),
            1 => result.First(),
            _ => throw new DuplicateEntityFoundException<TEntity>(key),
        };
    }

    /// <inheritdoc/>
    public Task<TEntity> GetSingleAsync(IPerCompanyPrimaryKey key, CancellationToken cancellationToken)
    {
        (string dataAreaId, IDictionary<string, object?> dict) = KeyToDictionary(key);
        return GetSingleAsync(dataAreaId, dict, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<TEntity> GetSingleAsync(ICommonPrimaryKey key, CancellationToken cancellationToken)
    {
        return GetSingleAsync(DefaultCompany, KeyToDictionary(key), cancellationToken);
    }

    /// <inheritdoc/>
    public Task<TEntity> GetSingleAsync(IDictionary<string, object?> keys, CancellationToken cancellationToken)
    {
        return GetSingleAsync(DefaultCompany, keys, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TEntity> GetSingleAsync(string company, IDictionary<string, object?> keys, CancellationToken cancellationToken)
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

            Logger.LogDebug("The method call to '{Path}' was successful.", url.AbsoluteUri);
            return content;
        }
        catch (Exception ex)
        {
            string? responseContent = (response == null) ? null : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            Logger.LogError(
                "Can't get {EntityName} with keys {Keys}. The method call to '{Path}' failed. response content :\n{ResponseContent}",
                typeof(TEntity).Name,
                keys,
                url.AbsoluteUri,
                responseContent ?? "No response");
            throw new GetSingleRequestFailedException<TEntity>(TEntity.EntityName(), keys, responseContent, message: null, ex);
        }
    }
}