// <copyright file="Dynamics365FinanceAndOperationsClient{Post}.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;

using Ardalis.GuardClauses;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Models;

using Microsoft.Extensions.Logging;

using System.Net.Http.Json;
using System.Runtime.Serialization;

/// <summary>
/// Client for Dynamics 365 for finance and operations.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public partial class Dynamics365FinanceAndOperationsClient<TEntity> : IDynamics365FinanceAndOperationsClient<TEntity>
    where TEntity : class, IODataElement
{
    /// <inheritdoc/>
    public async Task<TEntity> PostAsync<TCreate>(TCreate value, CancellationToken cancellationToken)
    {
        _ = Guard.Against.Null(value);
        TEntity result = await PostAsync(DefaultCompany, value, cancellationToken)
            .ConfigureAwait(false);
        return result;
    }

    /// <inheritdoc/>
    public async Task<TEntity> PostAsync<TCreate>(string company, TCreate value, CancellationToken cancellationToken)
    {
        _ = Guard.Against.Null(value);
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
    public Task<HttpResponseMessage> SendPostAsync<TCreate>(TCreate value, CancellationToken cancellationToken)
    {
        _ = Guard.Against.Null(value);
        return SendPostAsync(DefaultCompany, value, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<HttpResponseMessage> SendPostAsync<TCreate>(string company, TCreate value, CancellationToken cancellationToken)
    {
        _ = Guard.Against.NullOrWhiteSpace(company);
        _ = Guard.Against.Null(value);
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
}
