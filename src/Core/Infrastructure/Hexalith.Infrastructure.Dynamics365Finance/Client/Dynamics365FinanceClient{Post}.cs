// <copyright file="Dynamics365FinanceClient{Post}.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Client;

using System;
using System.Net.Http.Json;
using System.Text.Json;

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
    public async Task<TEntity> PostAsync<TCreate>(TCreate value, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(value);
        TEntity result = await PostAsync(DefaultCompany, value, cancellationToken)
            .ConfigureAwait(false);
        return result;
    }

    /// <inheritdoc/>
    public async Task<TEntity> PostAsync<TCreate>(string company, TCreate value, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(value);
        HttpResponseMessage? response = await SendPostAsync(company, value, cancellationToken).ConfigureAwait(false);
        TEntity? v = await response
            .Content
            .ReadFromJsonAsync<TEntity>(
                options: JsonOptions,
                cancellationToken).ConfigureAwait(false);
        return v ??
            throw new Dynamics365FinancePostException<TEntity, TCreate>(
                null,
                company,
                value,
                null,
                $"Empty content response on request to '{response.RequestMessage?.RequestUri?.AbsoluteUri}'.",
                await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false),
                null);
    }

    /// <inheritdoc/>
    public async Task<HttpResponseMessage> SendPostAsync<TCreate>(TCreate value, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(value);
        return await SendPostAsync(DefaultCompany, value, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<HttpResponseMessage> SendPostAsync<TCreate>(string company, TCreate value, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentException.ThrowIfNullOrWhiteSpace(company);
        string crossCompany = string.Equals(DefaultCompany, company, StringComparison.OrdinalIgnoreCase) ? string.Empty : "/?" + _crossCompanyQuery;
        Uri url = new(_instance, $"{_dataPath}/{TEntity.EntityName()}{crossCompany}");
        HttpResponseMessage? response = null;
        try
        {
            HttpClient client = await GetClientAsync(cancellationToken).ConfigureAwait(false);
            response = await client
                .PostAsJsonAsync(
                    url,
                    value,
                    options: JsonOptions,
                    cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                LogPostSucceededInformation(Logger, url.AbsolutePath);
                return response;
            }

            throw new HttpRequestException($"The post request failed with status code '{response?.StatusCode}'.");
        }
        catch (HttpRequestException e)
        {
            string? responseContent = response == null ? null : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            ErrorResponse? error = (responseContent == null) ? null : JsonSerializer.Deserialize<ErrorResponse>(responseContent);
            LogPostError(Logger, e, url.AbsoluteUri, responseContent);
            throw new Dynamics365FinancePostException<TEntity, TCreate>(url, company, value, error, $"The post failed.", responseContent, e);
        }
    }

    [LoggerMessage(EventId = 3, Level = LogLevel.Error, Message = "The post call to '{Path}' failed. response content :\n{ResponseContent}")]
    private static partial void LogPostError(ILogger logger, Exception e, string path, string? responseContent);

    [LoggerMessage(EventId = 4, Level = LogLevel.Information, Message = "The post method call to '{Path}' succeeded.")]
    private static partial void LogPostSucceededInformation(ILogger logger, string Path);
}