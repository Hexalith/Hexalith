// <copyright file="Dynamics365FinanceAndOperationsClient{Patch}.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Models;

using Microsoft.Extensions.Logging;

using System.Net.Http.Json;
using System.Web;

/// <summary>
/// Client for Dynamics 365 for finance and operations.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public partial class Dynamics365FinanceAndOperationsClient<TEntity> : IDynamics365FinanceAndOperationsClient<TEntity>
    where TEntity : class, IODataElement
{
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
}
