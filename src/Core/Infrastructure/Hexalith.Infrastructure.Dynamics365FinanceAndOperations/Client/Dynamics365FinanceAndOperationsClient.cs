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
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Client for Dynamics 365 for finance and operations.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public partial class Dynamics365FinanceAndOperationsClient<TEntity> : IDynamics365FinanceAndOperationsClient<TEntity>
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
    protected static JsonSerializerOptions JsonOptions => new()
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

    private static IDictionary<string, object?> ToDictionary<T>(T obj)
    {
        _ = Guard.Against.Null(obj);
        Dictionary<string, object?> dico = new(StringComparer.Ordinal);
        foreach (System.Reflection.PropertyInfo prop in obj.GetType().GetProperties())
        {
            dico.Add(prop.Name, prop.GetValue(obj));
        }

        return dico;
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
