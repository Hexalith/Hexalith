// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365FinanceAndOperations
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-06-2023
// ***********************************************************************
// <copyright file="Dynamics365FinanceAndOperationsClient.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;

using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Configurations;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Models;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Security;
using Hexalith.Infrastructure.Serialization.Serialization;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Client for Dynamics 365 for finance and operations.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public partial class Dynamics365FinanceAndOperationsClient<TEntity> : IDynamics365FinanceAndOperationsClient<TEntity>
    where TEntity : class, IODataElement
{
    /// <summary>
    /// The cross company query.
    /// </summary>
    private const string _crossCompanyQuery = "cross-company=true";

    /// <summary>
    /// The data path.
    /// </summary>
    private const string _dataPath = "data";

    /// <summary>
    /// The HTTP client factory.
    /// </summary>
    private readonly IHttpClientFactory _httpClientFactory;

    /// <summary>
    /// The instance.
    /// </summary>
    private readonly Uri _instance;

    /// <summary>
    /// The security context.
    /// </summary>
    private readonly IDynamics365FinanceAndOperationsSecurityContext _securityContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceAndOperationsClient{TEntity}" /> class.
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
        : this(httpClientFactory, securityContext, settings, (ILogger)logger)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceAndOperationsClient{TEntity}" /> class.
    /// </summary>
    /// <param name="httpClientFactory">The HTTP client factory.</param>
    /// <param name="securityContext">The security context.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentException">The {nameof(s.Instance)} setting is not defined. - settings.</exception>
    /// <exception cref="System.ArgumentException">The {nameof(s.Company)} setting is not defined. - settings.</exception>
    protected Dynamics365FinanceAndOperationsClient(
        IHttpClientFactory httpClientFactory,
        IDynamics365FinanceAndOperationsSecurityContext securityContext,
        IOptions<Dynamics365FinanceAndOperationsClientSettings> settings,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(httpClientFactory);
        ArgumentNullException.ThrowIfNull(securityContext);
        ArgumentNullException.ThrowIfNull(logger);

        Dynamics365FinanceAndOperationsClientSettings s = settings.Value;

        _httpClientFactory = httpClientFactory;
        _securityContext = securityContext;
        Logger = logger;
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

    /// <summary>
    /// Gets the json options.
    /// </summary>
    /// <value>The json options.</value>
    public static JsonSerializerOptions JsonOptions => new()
    {
        PropertyNameCaseInsensitive = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters =
        {
            new IsoUtcDateTimeOffsetConverter(),
        },
    };

    /// <inheritdoc/>
    public string DefaultCompany { get; }

    /// <summary>
    /// Gets the logger.
    /// </summary>
    /// <value>The logger.</value>
    protected ILogger Logger { get; }

    /// <inheritdoc/>
    public Task DoActionAsync(string action, IDictionary<string, object?> parameters, CancellationToken cancellationToken) => throw new NotSupportedException();

    /// <summary>
    /// Per company filters to dictionary.
    /// </summary>
    /// <param name="filter">The key.</param>
    /// <returns>System.ValueTuple&lt;System.String, IDictionary&lt;System.String, System.Nullable&lt;System.Object&gt;&gt;&gt;.</returns>
    protected virtual (string DataAreaId, IDictionary<string, object?> Values) FilterToDictionary(IPerCompanyFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);
        IDictionary<string, object?> dict = ToDictionary(filter);
        if (dict.Count < 2)
        {
            throw new ArgumentException(
                "The filter must have at least one property other than the DataAreaId.",
                nameof(filter));
        }

        string dataAreaId = string.IsNullOrWhiteSpace(filter.DataAreaId) ? DefaultCompany : filter.DataAreaId;
        _ = dict.Remove(nameof(IPerCompanyFilter.DataAreaId));
        return (dataAreaId, dict);
    }

    /// <summary>
    /// Common filters to dictionary.
    /// </summary>
    /// <param name="filter">The filter.</param>
    /// <returns>IDictionary&lt;System.String, System.Nullable&lt;System.Object&gt;&gt;.</returns>
    protected virtual IDictionary<string, object?> FilterToDictionary(ICommonFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);
        IDictionary<string, object?> dict = ToDictionary(filter);
        return dict.Count < 1
            ? throw new ArgumentException(
                "The filter must have at least one property.",
                nameof(filter))
            : dict;
    }

    /// <summary>
    /// Keys to dictionary.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>System.ValueTuple&lt;System.String, IDictionary&lt;System.String, System.Nullable&lt;System.Object&gt;&gt;&gt;.</returns>
    protected virtual (string DataAreaId, IDictionary<string, object?> Values) KeyToDictionary(IPerCompanyPrimaryKey key)
    {
        ArgumentNullException.ThrowIfNull(key);
        IDictionary<string, object?> dict = ToDictionary(key);
        if (dict.Count < 2)
        {
            throw new ArgumentException(
                "The key must have at least one property other than the DataAreaId.",
                nameof(key));
        }

        string dataAreaId = string.IsNullOrWhiteSpace(key.DataAreaId) ? DefaultCompany : key.DataAreaId;
        _ = dict.Remove(nameof(IPerCompanyPrimaryKey.DataAreaId));
        return (dataAreaId, dict);
    }

    /// <summary>
    /// Keys to dictionary.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>IDictionary&lt;System.String, System.Nullable&lt;System.Object&gt;&gt;.</returns>
    protected virtual IDictionary<string, object?> KeyToDictionary(ICommonPrimaryKey key)
    {
        ArgumentNullException.ThrowIfNull(key);
        IDictionary<string, object?> dict = ToDictionary(key);
        return dict.Count < 1
            ? throw new ArgumentException(
                "The key must have at least one property.",
                nameof(key))
            : dict;
    }

    /// <summary>
    /// Gets the entity filter.
    /// </summary>
    /// <param name="company">The company.</param>
    /// <param name="keys">The keys.</param>
    /// <returns>System.String.</returns>
    private static string GetEntityFilter(string company, IDictionary<string, object?> keys)
    {
        StringBuilder filter = new();
        _ = filter.Append(CultureInfo.InvariantCulture, $"dataAreaId='{company}'");
        foreach (KeyValuePair<string, object?> key in keys)
        {
            _ = filter.Append(CultureInfo.InvariantCulture, $",{key.Key}={GetODataString(key.Value)}");
        }

        return filter.ToString();
    }

    /// <summary>
    /// Gets the OData string.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>System.String.</returns>
    private static string GetODataString(object? value)
    {
        string v = value is string s ? $"'{s}'" : Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
        return value is null ? "null" : v;
    }

    /// <summary>
    /// Gets the query filter.
    /// </summary>
    /// <param name="company">The company.</param>
    /// <param name="filter">The filter.</param>
    /// <returns>System.String.</returns>
    private static string GetQueryFilter(string company, IDictionary<string, object?> filter)
    {
        StringBuilder query = new();
        _ = query.Append(CultureInfo.InvariantCulture, $"dataAreaId eq '{company}'");
        foreach (KeyValuePair<string, object?> key in filter)
        {
            _ = query.Append(CultureInfo.InvariantCulture, $" and {key.Key} eq {GetODataString(key.Value)}");
        }

        return query.ToString();
    }

    /// <summary>
    /// Converts to dictionary.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    /// <param name="obj">The object.</param>
    /// <returns>IDictionary&lt;System.String, System.Nullable&lt;System.Object&gt;&gt;.</returns>
    private static IDictionary<string, object?> ToDictionary<T>(T obj)
    {
        ArgumentNullException.ThrowIfNull(obj);
        Dictionary<string, object?> dico = new(StringComparer.Ordinal);
        foreach (System.Reflection.PropertyInfo prop in obj.GetType().GetProperties())
        {
            dico.Add(prop.Name, prop.GetValue(obj));
        }

        return dico;
    }

    /// <summary>
    /// Add request headers as an asynchronous operation.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="System.InvalidOperationException">The acquired token is null or empty.</exception>
    private async Task<HttpClient> GetClientAsync(CancellationToken cancellationToken)
    {
        string token = await _securityContext
            .AcquireTokenAsync(cancellationToken)
            .ConfigureAwait(false);

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new InvalidOperationException("The acquired token is null or empty.");
        }

        HttpClient client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
        client.DefaultRequestHeaders.Add("OData-Version", "4.0");
        client.DefaultRequestHeaders.Add("Prefer", "odata.include-annotations = *");
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            token);
        return client;
    }
}