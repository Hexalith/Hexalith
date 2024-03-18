// <copyright file="Dynamics365FinanceClient{Patch}.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Client;

using System.Net.Http.Json;
using System.Text.Json;
using System.Web;

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
    public async Task PatchAsync<TUpdate>(IPerCompanyPrimaryKey key, TUpdate value, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);
        (string? dataAreaId, IDictionary<string, object?>? dict) = KeyToDictionary(key);
        await PatchAsync(dataAreaId, dict, value, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task PatchAsync<TUpdate>(ICommonPrimaryKey key, TUpdate value, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);
        await PatchAsync(DefaultCompany, KeyToDictionary(key), value, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task PatchAsync<TUpdate>(
        IDictionary<string, object?> key,
        TUpdate value,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);
        await PatchAsync(DefaultCompany, key, value, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task PatchAsync<TUpdate>(
        string company,
        IDictionary<string, object?> key,
        TUpdate value,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);
        ArgumentException.ThrowIfNullOrWhiteSpace(company);
        _ = await SendPatchAsync(company, key, value, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<HttpResponseMessage> SendPatchAsync<TUpdate>(
        IDictionary<string, object?> key,
        TUpdate value,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);
        return await SendPatchAsync(DefaultCompany, key, value, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<HttpResponseMessage> SendPatchAsync<TUpdate>(
        string company,
        IDictionary<string, object?> key,
        TUpdate value,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);
        ArgumentException.ThrowIfNullOrWhiteSpace(company);
        string crossCompany = string.Equals(DefaultCompany, company, StringComparison.OrdinalIgnoreCase) ? string.Empty : "?" + _crossCompanyQuery;
        Uri url = new(_instance, $"{_dataPath}/{TEntity.EntityName()}({HttpUtility.UrlEncode(GetEntityFilter(company, key))}){crossCompany}");
        HttpResponseMessage? response = null;
        try
        {
            HttpClient client = await GetClientAsync(cancellationToken).ConfigureAwait(false);
            response = await client
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
                LogPatchSucceededInformation(Logger, url.AbsolutePath);
                return response;
            }

            throw new HttpRequestException($"The patch request failed with status code '{response.StatusCode}'.");
        }
        catch (HttpRequestException e)
        {
            string? responseContent = response == null ? null : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            ErrorResponse? error = (responseContent == null) ? null : JsonSerializer.Deserialize<ErrorResponse>(responseContent);
            LogPatchError(Logger, e, url.AbsoluteUri, responseContent);
            throw new Dynamics365FinancePatchException<TEntity, TUpdate>(url, company, value, error, $"The patch failed.", responseContent, e);
        }
    }

    [LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "The patch call to '{Path}' failed. response content :\n{ResponseContent}")]
    private static partial void LogPatchError(ILogger logger, Exception e, string path, string? responseContent);

    [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "The patch method call to '{Path}' succeeded.")]
    private static partial void LogPatchSucceededInformation(ILogger logger, string Path);
}