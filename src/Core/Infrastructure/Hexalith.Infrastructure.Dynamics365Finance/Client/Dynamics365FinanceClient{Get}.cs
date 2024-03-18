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
    /// <summary>
    /// Logs the cant get entity with filter error.
    /// </summary>
    /// <param name="e">The e.</param>
    /// <param name="entityName">Name of the entity.</param>
    /// <param name="filter">The filter.</param>
    /// <param name="path">The path.</param>
    /// <param name="responseContent">Content of the response.</param>
    [LoggerMessage(EventId = 2, Level = LogLevel.Error, Message = "Can't get {EntityName} list with filter {Filter}. The method call to '{Path}' failed. response content :\n{ResponseContent}")]
    public static partial void LogCantGetEntityWithFilterError(ILogger logger, Exception e, string entityName, string filter, string path, string responseContent);

    /// <summary>
    /// Logs the cant get entity with key error.
    /// </summary>
    /// <param name="e">The e.</param>
    /// <param name="entityName">Name of the entity.</param>
    /// <param name="keys">The keys.</param>
    /// <param name="path">The path.</param>
    /// <param name="responseContent">Content of the response.</param>
    [LoggerMessage(EventId = 3, Level = LogLevel.Error, Message = "Can't get {EntityName} with keys {Keys}. The method call to '{Path}' failed. response content :\n{ResponseContent}")]
    public static partial void LogCantGetEntityWithKeyError(ILogger logger, Exception e, string entityName, string keys, string path, string responseContent);

    /// <summary>
    /// Logs the dynamics odata call information.
    /// </summary>
    /// <param name="path">The path.</param>
    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Calling Dynamics 365 Finance ODATA endpoint : {Path}.")]
    public static partial void LogDynamicsOdataCallInformation(ILogger logger, string path);

    /// <summary>
    /// Logs the method call successful debug information.
    /// </summary>
    /// <param name="path">The path.</param>
    [LoggerMessage(EventId = 4, Level = LogLevel.Debug, Message = "The method call to '{Path}' was successful.")]
    public static partial void LogMethodCallSuccessfulDebugInformation(ILogger logger, string path);

    /// <summary>
    /// Logs the method call to path was successful trace information.
    /// </summary>
    /// <param name="path">The path.</param>
    [LoggerMessage(EventId = 0, Level = LogLevel.Debug, Message = "Calling Dynamics 365 Finance ODATA endpoint : {Path}.")]
    public static partial void LogTheMethodCallToPathWasSuccessfulTraceInformation(ILogger logger, string path);

    /// <inheritdoc/>
    public async Task<int> CountAsync(IPerCompanyFilter filter, CancellationToken cancellationToken)
    {
        (string dataAreaId, IDictionary<string, object?> dict) = FilterToDictionary(filter);
        IEnumerable<TEntity> result = await GetPerCompanyAsync(dataAreaId, dict, cancellationToken).ConfigureAwait(false);
        return result.Count();
    }

    /// <inheritdoc/>
    public async Task<int> CountAsync(ICommonFilter filter, CancellationToken cancellationToken)
    {
        IEnumerable<TEntity> result = await GetCommonAsync(FilterToDictionary(filter), cancellationToken).ConfigureAwait(false);
        return result.Count();
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(IPerCompanyPrimaryKey key, CancellationToken cancellationToken)
    {
        (string dataAreaId, IDictionary<string, object?> dict) = KeyToDictionary(key);
        IEnumerable<TEntity> result = await GetPerCompanyAsync(dataAreaId, dict, cancellationToken).ConfigureAwait(false);
        int count = result.Count();
        return count == 1 || (count == 0 ? false : throw new InvalidOperationException($"The key {JsonSerializer.Serialize(key)} is not unique."));
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(ICommonPrimaryKey key, CancellationToken cancellationToken)
    {
        IEnumerable<TEntity> result = await GetCommonAsync(KeyToDictionary(key), cancellationToken).ConfigureAwait(false);
        int count = result.Count();
        return count == 1 || (count == 0 ? false : throw new InvalidOperationException($"The key {JsonSerializer.Serialize(key)} is not unique."));
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(IPerCompanyFilter filter, CancellationToken cancellationToken) => await CountAsync(filter, cancellationToken).ConfigureAwait(false) > 0;

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(ICommonFilter filter, CancellationToken cancellationToken) => await CountAsync(filter, cancellationToken).ConfigureAwait(false) > 0;

    /// <inheritdoc/>
    public async Task<IEnumerable<TEntity>> GetAsync(IDictionary<string, object?> filter, CancellationToken cancellationToken)
        => _isPerCompany
            ? await GetPerCompanyAsync(DefaultCompany, filter, cancellationToken).ConfigureAwait(false)
            : await GetCommonAsync(filter, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Get as an asynchronous operation.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;System.Collections.Generic.IEnumerable.<TEntity>&gt; representing the asynchronous operation.</returns>
    /// <exception cref="HttpRequestException">$"The request to '{url.AbsoluteUri}' failed with status code '{response.StatusCode}'.</exception>
    /// <exception cref="HttpRequestException">$"The request to '{url.AbsoluteUri}' failed to deserialize :\n{await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)}</exception>
    public async Task<IEnumerable<TEntity>> GetAsync(Uri url, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(url);
        HttpResponseMessage? response = null;
        try
        {
            LogDynamicsOdataCallInformation(Logger, url.AbsoluteUri);

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
                LogMethodCallSuccessfulDebugInformation(Logger, url.AbsoluteUri);
                return content.Values ?? Enumerable.Empty<TEntity>();
            }

            throw new HttpRequestException(
                    $"The request to '{url.AbsoluteUri}' failed to deserialize :\n{await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)}");
        }
        catch (Exception ex)
        {
            string? responseContent = (response == null) ? null : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            LogCantGetEntityWithFilterError(
                Logger,
                ex,
                TEntity.EntityName(),
                string.Empty,
                url.AbsoluteUri,
                responseContent ?? "No response");
            throw new GetRequestFailedException<TEntity>(TEntity.EntityName(), url, responseContent, message: null, ex);
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TEntity>> GetAsync(IPerCompanyFilter filter, CancellationToken cancellationToken)
    {
        (string dataAreaId, IDictionary<string, object?> dict) = FilterToDictionary(filter);
        return await GetPerCompanyAsync(dataAreaId, dict, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TEntity>> GetAsync(ICommonFilter filter, CancellationToken cancellationToken)
        => await GetCommonAsync(FilterToDictionary(filter), cancellationToken).ConfigureAwait(false);

    /// <inheritdoc/>
    public async Task<IEnumerable<TEntity>> GetCommonAsync([NotNull] IDictionary<string, object?> filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        Uri url = new(_instance, $"{_dataPath}/{TEntity.EntityName()}/?$filter={HttpUtility.UrlEncode(GetQueryFilter(filter))}");
        return await GetAsync(url, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TEntity>> GetPerCompanyAsync(string company, [NotNull] IDictionary<string, object?> filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        string crossCompany = string.Equals(DefaultCompany, company, StringComparison.OrdinalIgnoreCase) ? string.Empty : _crossCompanyQuery + "&";
        filter.Add("dataAreaId", company);
        Uri url = new(_instance, $"{_dataPath}/{TEntity.EntityName()}/?{crossCompany}$filter={HttpUtility.UrlEncode(GetQueryFilter(filter))}");
        return await GetAsync(url, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<TEntity> GetSingleAsync(IPerCompanyFilter key, CancellationToken cancellationToken)
    {
        IEnumerable<TEntity> result = await GetAsync(key, cancellationToken).ConfigureAwait(false);
        return result.Count() switch
        {
            0 => throw new EntityNotFoundException<TEntity>(key),
            1 => result.First(),
            _ => throw new DuplicateEntityFoundException<TEntity>(key, result),
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
            _ => throw new DuplicateEntityFoundException<TEntity>(key, result),
        };
    }

    /// <inheritdoc/>
    public async Task<TEntity> GetSingleAsync(IPerCompanyPrimaryKey key, CancellationToken cancellationToken)
    {
        (string dataAreaId, IDictionary<string, object?> dict) = KeyToDictionary(key);
        return await GetSingleAsync(dataAreaId, dict, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<TEntity> GetSingleAsync(ICommonPrimaryKey key, CancellationToken cancellationToken) => await GetSingleAsync(DefaultCompany, KeyToDictionary(key), cancellationToken).ConfigureAwait(false);

    /// <inheritdoc/>
    public async Task<TEntity> GetSingleAsync(IDictionary<string, object?> keys, CancellationToken cancellationToken)
        => await GetSingleAsync(DefaultCompany, keys, cancellationToken).ConfigureAwait(false);

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
            LogMethodCallSuccessfulDebugInformation(Logger, url.AbsoluteUri);
            return content;
        }
        catch (Exception ex)
        {
            string? responseContent = (response == null) ? null : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            LogCantGetEntityWithKeyError(
                Logger,
                ex,
                TEntity.EntityName(),
                string.Join('\n', keys.Select(s => $"{s.Key}='{s.Value}'")),
                url.AbsoluteUri,
                responseContent ?? "No response");
            throw new GetSingleRequestFailedException<TEntity>(TEntity.EntityName(), keys, responseContent, message: null, ex);
        }
    }
}