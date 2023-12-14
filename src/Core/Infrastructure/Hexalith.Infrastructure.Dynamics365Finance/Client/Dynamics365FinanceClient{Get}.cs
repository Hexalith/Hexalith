// <copyright file="Dynamics365FinanceClient{Get}.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Client;

using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Text.Json;
using System.Web;

using Hexalith.Application.Exceptions;
using Hexalith.Infrastructure.Dynamics365Finance.Models;

using Microsoft.Extensions.Logging;

/// <summary>
/// Client for Dynamics 365 for finance and operations.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public partial class Dynamics365FinanceClient<TEntity> : IDynamics365FinanceClient<TEntity>
    where TEntity : class, IODataCommon
{
    /// <inheritdoc/>
    public async Task<int> CountAsync(IPerCompanyFilter filter, CancellationToken cancellationToken)
    {
        (string dataAreaId, IDictionary<string, object?> dict) = FilterToDictionary(filter);
        IEnumerable<TEntity> result = await GetAsync(dataAreaId, dict, cancellationToken).ConfigureAwait(false);
        return result.Count();
    }

    /// <inheritdoc/>
    public async Task<int> CountAsync(ICommonFilter filter, CancellationToken cancellationToken)
    {
        IEnumerable<TEntity> result = await GetAsync(DefaultCompany, FilterToDictionary(filter), cancellationToken).ConfigureAwait(false);
        return result.Count();
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(IPerCompanyPrimaryKey key, CancellationToken cancellationToken)
    {
        (string dataAreaId, IDictionary<string, object?> dict) = KeyToDictionary(key);
        IEnumerable<TEntity> result = await GetAsync(dataAreaId, dict, cancellationToken).ConfigureAwait(false);
        int count = result.Count();
        return count == 1 || (count == 0 ? false : throw new InvalidOperationException($"The key {JsonSerializer.Serialize(key)} is not unique."));
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(ICommonPrimaryKey key, CancellationToken cancellationToken)
    {
        IEnumerable<TEntity> result = await GetAsync(DefaultCompany, KeyToDictionary(key), cancellationToken).ConfigureAwait(false);
        int count = result.Count();
        return count == 1 || (count == 0 ? false : throw new InvalidOperationException($"The key {JsonSerializer.Serialize(key)} is not unique."));
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(IPerCompanyFilter filter, CancellationToken cancellationToken) => await CountAsync(filter, cancellationToken).ConfigureAwait(false) > 0;

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(ICommonFilter filter, CancellationToken cancellationToken) => await CountAsync(filter, cancellationToken).ConfigureAwait(false) > 0;

    /// <inheritdoc/>
    public Task<IEnumerable<TEntity>> GetAsync(IDictionary<string, object?> filter, CancellationToken cancellationToken) => GetAsync(DefaultCompany, filter, cancellationToken);

    /// <inheritdoc/>
    public async Task<IEnumerable<TEntity>> GetAsync(string company, [NotNull] IDictionary<string, object?> filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        string crossCompany = string.Equals(DefaultCompany, company, StringComparison.OrdinalIgnoreCase) ? string.Empty : _crossCompanyQuery + "&";
        Uri url = new(_instance, $"{_dataPath}/{TEntity.EntityName()}/?{crossCompany}$filter={HttpUtility.UrlEncode(GetQueryFilter(company, filter))}");
        HttpResponseMessage? response = null;
        try
        {
            Logger.LogInformation("Calling Dynamics 365 Finance ODATA endpoint : {Path}.", url.AbsoluteUri);

            HttpClient client = await GetClientAsync(cancellationToken).ConfigureAwait(false);
            response = await client.GetAsync(url, cancellationToken).ConfigureAwait(false);
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
        catch (Exception ex)
        {
            string? responseContent = (response == null) ? null : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            Logger.LogError(
                "Can't get {EntityName} list with filter {Filter}. The method call to '{Path}' failed. response content :\n{ResponseContent}",
                typeof(TEntity).Name,
                JsonSerializer.Serialize(filter),
                url.AbsoluteUri,
                responseContent ?? "No response");
            throw new GetRequestFailedException<TEntity>(TEntity.EntityName(), filter, responseContent, message: null, ex);
        }
    }

    /// <inheritdoc/>
    public Task<IEnumerable<TEntity>> GetAsync(IPerCompanyFilter filter, CancellationToken cancellationToken)
    {
        (string dataAreaId, IDictionary<string, object?> dict) = FilterToDictionary(filter);
        return GetAsync(dataAreaId, dict, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<IEnumerable<TEntity>> GetAsync(ICommonFilter filter, CancellationToken cancellationToken) => GetAsync(DefaultCompany, FilterToDictionary(filter), cancellationToken);

    /// <inheritdoc/>
    public async Task<TEntity> GetSingleAsync(IPerCompanyFilter key, CancellationToken cancellationToken)
    {
        IEnumerable<TEntity> result = await GetAsync(key, cancellationToken).ConfigureAwait(false);
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
        IEnumerable<TEntity> result = await GetAsync(key, cancellationToken).ConfigureAwait(false);
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
    public Task<TEntity> GetSingleAsync(ICommonPrimaryKey key, CancellationToken cancellationToken) => GetSingleAsync(DefaultCompany, KeyToDictionary(key), cancellationToken);

    /// <inheritdoc/>
    public Task<TEntity> GetSingleAsync(IDictionary<string, object?> keys, CancellationToken cancellationToken) => GetSingleAsync(DefaultCompany, keys, cancellationToken);

    /// <inheritdoc/>
    public async Task<TEntity> GetSingleAsync(string company, [NotNull] IDictionary<string, object?> keys, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(keys);
        string crossCompany = string.Equals(DefaultCompany, company, StringComparison.OrdinalIgnoreCase) ? string.Empty : "?" + _crossCompanyQuery;
        string keyFilter = GetEntityFilter(company, keys);
        Uri url = new(_instance, $"{_dataPath}/{TEntity.EntityName()}({HttpUtility.UrlEncode(keyFilter)}){crossCompany}");
        HttpResponseMessage? response = null;
        try
        {
            HttpClient client = await GetClientAsync(cancellationToken).ConfigureAwait(false);
            response = await client.GetAsync(url, cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"The request to '{url.AbsoluteUri}' failed with status code '{response.StatusCode}'.");
            }

            TEntity? content = await response
                    .Content
                    .ReadFromJsonAsync<TEntity>(
                    options: JsonOptions,
                    cancellationToken).ConfigureAwait(false) ?? throw new HttpRequestException($"Empty content response on request to '{url.AbsoluteUri}'.");
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