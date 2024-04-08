// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-06-2023
// ***********************************************************************
// <copyright file="Dynamics365FinanceClient.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Client;

using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using Hexalith.Application.Organizations.Configurations;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.Dynamics365Finance.Configurations;
using Hexalith.Infrastructure.Dynamics365Finance.Models;
using Hexalith.Infrastructure.Dynamics365Finance.Security;
using Hexalith.Infrastructure.Serialization.Serialization;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Client for Dynamics 365 for finance and operations.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public partial class Dynamics365FinanceClient<TEntity> : IDynamics365FinanceClient<TEntity>
    where TEntity : class, IODataCommon
{
    /// <summary>
    /// Gets the logger.
    /// </summary>
    /// <value>The logger.</value>
    protected ILogger Logger;

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
    private readonly HttpClient _httpClient;

    /// <summary>
    /// The instance.
    /// </summary>
    private readonly Uri _instance;

    /// <summary>
    /// The is per company.
    /// </summary>
    private readonly bool _isPerCompany = typeof(IODataElement).IsAssignableFrom(typeof(TEntity));

    /// <summary>
    /// The security context.
    /// </summary>
    private readonly IDynamics365FinanceSecurityContext _securityContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceClient{TEntity}" /> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client factory.</param>
    /// <param name="securityContext">The security context.</param>
    /// <param name="finOpsSettings">The fin ops settings.</param>
    /// <param name="organizationSettings">The organization settings.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    /// <exception cref="System.ArgumentException">The {nameof(finOpsSettings.Value.Instance)} setting is not defined. - finOpsSettings.</exception>
    public Dynamics365FinanceClient(
        HttpClient httpClient,
        IDynamics365FinanceSecurityContext securityContext,
        IOptions<Dynamics365FinanceClientSettings> finOpsSettings,
        IOptions<OrganizationSettings> organizationSettings,
        ILogger<Dynamics365FinanceClient<TEntity>> logger)
    {
        ArgumentNullException.ThrowIfNull(finOpsSettings);
        ArgumentNullException.ThrowIfNull(organizationSettings);
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(securityContext);
        ArgumentNullException.ThrowIfNull(logger);

        SettingsException<OrganizationSettings>.ThrowIfNullOrEmpty(organizationSettings.Value.DefaultCompanyId);
        SettingsException<Dynamics365FinanceClientSettings>.ThrowIfUndefined(finOpsSettings.Value.Instance);

        _httpClient = httpClient;
        _securityContext = securityContext;
        Logger = logger;
        if (string.IsNullOrWhiteSpace(finOpsSettings.Value.Instance?.OriginalString))
        {
            throw new ArgumentException(
                $"The {nameof(finOpsSettings.Value.Instance)} setting is not defined.",
                nameof(finOpsSettings));
        }

        DefaultCompany = organizationSettings.Value.DefaultCompanyId;
        _instance = finOpsSettings.Value.Instance;
    }

    /// <summary>
    /// Gets the json options.
    /// </summary>
    /// <value>The json options.</value>
#pragma warning disable CA1000 // Do not declare static members on generic types

    public static JsonSerializerOptions JsonOptions => new()
    {
        PropertyNameCaseInsensitive = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters =
        {
            new IsoUtcDateTimeOffsetConverter(),
        },
    };

#pragma warning restore CA1000 // Do not declare static members on generic types

    /// <inheritdoc/>
    public string DefaultCompany { get; }

    /// <inheritdoc/>
    public async Task DoActionAsync(string action, IDictionary<string, object?> parameters, CancellationToken cancellationToken)
    {
        await Task.CompletedTask.ConfigureAwait(false);
        throw new NotSupportedException();
    }

    /// <summary>
    /// Get as an asynchronous operation.
    /// </summary>
    /// <param name="company">The company.</param>
    /// <param name="filter">The filter.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;IEnumerable`1&gt; representing the asynchronous operation.</returns>
    /// <exception cref="System.NotImplementedException">null.</exception>
    public async Task<IEnumerable<TEntity>> GetAsync(string company, IDictionary<string, object?> filter, CancellationToken cancellationToken)
    {
        await Task.CompletedTask.ConfigureAwait(false);
        throw new NotImplementedException();
    }

    /// <summary>
    /// Per company filters to dictionary.
    /// </summary>
    /// <param name="filter">The key.</param>
    /// <returns>System.ValueTuple&lt;System.String, IDictionary&lt;System.String, System.Nullable&lt;System.Object&gt;&gt;&gt;.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    /// <exception cref="System.ArgumentException">The filter must have at least one property other than the DataAreaId. - filter.</exception>
    protected virtual (string DataAreaId, IDictionary<string, object?> Values) FilterToDictionary(IPerCompanyFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);
        Dictionary<string, object?> dict = ToDictionary(filter);
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
    /// <exception cref="System.ArgumentNullException">null.</exception>
    /// <exception cref="System.ArgumentException">The filter must have at least one property. - filter.</exception>
    protected virtual IDictionary<string, object?> FilterToDictionary(ICommonFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);
        Dictionary<string, object?> dict = ToDictionary(filter);
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
    /// <exception cref="System.ArgumentNullException">null.</exception>
    /// <exception cref="System.ArgumentException">The key must have at least one property other than the DataAreaId. - key.</exception>
    protected virtual (string DataAreaId, IDictionary<string, object?> Values) KeyToDictionary(IPerCompanyPrimaryKey key)
    {
        ArgumentNullException.ThrowIfNull(key);
        Dictionary<string, object?> dict = ToDictionary(key);
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
    /// <exception cref="System.ArgumentNullException">null.</exception>
    /// <exception cref="System.ArgumentException">The key must have at least one property. - key.</exception>
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
    /// Gets the debugger display.
    /// </summary>
    /// <returns>System.String.</returns>
    private static string GetDebuggerDisplay() => typeof(TEntity).Name;

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
    /// <param name="filter">The filter.</param>
    /// <returns>System.String.</returns>
    private static string GetQueryFilter(IDictionary<string, object?> filter)
    {
        StringBuilder query = new();
        foreach (KeyValuePair<string, object?> key in filter)
        {
            if (query.Length > 0)
            {
                _ = query.Append(CultureInfo.InvariantCulture, $" and ");
            }

            _ = query.Append(CultureInfo.InvariantCulture, $"{key.Key} eq {GetODataString(key.Value)}");
        }

        return query.ToString();
    }

    /// <summary>
    /// Converts to dictionary.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    /// <param name="obj">The object.</param>
    /// <returns>IDictionary&lt;System.String, System.Nullable&lt;System.Object&gt;&gt;.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    private static Dictionary<string, object?> ToDictionary<T>(T obj)
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
        _httpClient.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue(
           "Bearer",
           token);

        return _httpClient;
    }
}