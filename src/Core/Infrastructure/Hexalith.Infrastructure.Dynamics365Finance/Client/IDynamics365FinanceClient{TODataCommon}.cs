// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance
// Author           : Jérôme Piquot
// Created          : 10-03-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-14-2023
// ***********************************************************************
// <copyright file="IDynamics365FinanceClient{TODataCommon}.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Client;

using System.Diagnostics.CodeAnalysis;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Dynamics 365 Finance and Operations client interface.
/// </summary>
/// <typeparam name="TODataCommon">The entity record type.</typeparam>
public interface IDynamics365FinanceClient<TODataCommon>
    where TODataCommon : IODataCommon
{
    /// <summary>
    /// Gets the connection default company.
    /// </summary>
    /// <value>The connection default company.</value>
    string DefaultCompany { get; }

    /// <summary>
    /// Counts the number of occurrences.
    /// </summary>
    /// <param name="filter">The filter.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Int32&gt;.</returns>
    Task<int> CountAsync(IPerCompanyFilter filter, CancellationToken cancellationToken);

    /// <summary>
    /// Counts the asynchronous.
    /// </summary>
    /// <param name="filter">The filter.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Int32&gt;.</returns>
    Task<int> CountAsync(ICommonFilter filter, CancellationToken cancellationToken);

    /// <summary>
    /// Execute an action on a Dynamics 365 Finance and Operations entity.
    /// </summary>
    /// <param name="action">Action name.</param>
    /// <param name="parameters">Action parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task.</returns>
    Task DoActionAsync(string action, IDictionary<string, object?> parameters, CancellationToken cancellationToken);

    /// <summary>
    /// Check if entity exists.
    /// </summary>
    /// <param name="filter">The filter.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Boolean&gt;.</returns>
    Task<bool> ExistsAsync(IPerCompanyFilter filter, CancellationToken cancellationToken);

    /// <summary>
    /// Counts the number of occurrences.
    /// </summary>
    /// <param name="filter">The filter.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Boolean&gt;.</returns>
    Task<bool> ExistsAsync(ICommonFilter filter, CancellationToken cancellationToken);

    /// <summary>
    /// Check if entity exists.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Boolean&gt;.</returns>
    Task<bool> ExistsAsync(ICommonPrimaryKey key, CancellationToken cancellationToken);

    /// <summary>
    /// Check if entity exists.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Boolean&gt;.</returns>
    Task<bool> ExistsAsync(IPerCompanyPrimaryKey key, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the asynchronous.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IEnumerable&lt;TODataElement&gt;&gt;.</returns>
    Task<IEnumerable<TODataCommon>> GetAsync(IPerCompanyFilter key, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the asynchronous.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IEnumerable&lt;TODataElement&gt;&gt;.</returns>
    Task<IEnumerable<TODataCommon>> GetAsync(ICommonFilter key, CancellationToken cancellationToken);

    /// <summary>
    /// Get a filtered entity object list.
    /// </summary>
    /// <param name="filter">Filter values.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of entity objects.</returns>
    Task<IEnumerable<TODataCommon>> GetAsync(IDictionary<string, object?> filter, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the common asynchronous.
    /// </summary>
    /// <param name="filter">The filter.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IEnumerable&lt;TODataCommon&gt;&gt;.</returns>
    Task<IEnumerable<TODataCommon>> GetCommonAsync([NotNull] IDictionary<string, object?> filter, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the per company asynchronous.
    /// </summary>
    /// <param name="company">The company.</param>
    /// <param name="filter">The filter.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IEnumerable&lt;TODataCommon&gt;&gt;.</returns>
    Task<IEnumerable<TODataCommon>> GetPerCompanyAsync(string company, [NotNull] IDictionary<string, object?> filter, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the single asynchronous.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;TODataElement&gt;.</returns>
    Task<TODataCommon> GetSingleAsync(IPerCompanyFilter key, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the single asynchronous.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;TODataElement&gt;.</returns>
    Task<TODataCommon> GetSingleAsync(ICommonFilter key, CancellationToken cancellationToken);

    /// <summary>
    /// Get entity object by it's primary key.
    /// </summary>
    /// <param name="keys">Primary key values.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Entity object.</returns>
    Task<TODataCommon> GetSingleAsync(IDictionary<string, object?> keys, CancellationToken cancellationToken);

    /// <summary>
    /// Get entity object by it's primary key.
    /// </summary>
    /// <param name="company">Company identifier.</param>
    /// <param name="keys">Primary key values.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Entity object.</returns>
    Task<TODataCommon> GetSingleAsync(string company, IDictionary<string, object?> keys, CancellationToken cancellationToken);

    /// <summary>
    /// Get entity object by it's primary key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Entity object.</returns>
    Task<TODataCommon> GetSingleAsync(ICommonPrimaryKey key, CancellationToken cancellationToken);

    /// <summary>
    /// Get per company entity object by it's primary key.
    /// </summary>
    /// <param name="key">The per company key.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Entity object.</returns>
    Task<TODataCommon> GetSingleAsync(IPerCompanyPrimaryKey key, CancellationToken cancellationToken);

    /// <summary>
    /// Patch an entity object.
    /// </summary>
    /// <typeparam name="TUpdate">Type of the update entity model.</typeparam>
    /// <param name="key">Primary key values.</param>
    /// <param name="value">Values to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The entity valued task.</returns>
    Task PatchAsync<TUpdate>(IDictionary<string, object?> key, TUpdate value, CancellationToken cancellationToken);

    /// <summary>
    /// Patch an entity object.
    /// </summary>
    /// <typeparam name="TUpdate">Type of the update entity model.</typeparam>
    /// <param name="company">Company identifier.</param>
    /// <param name="key">Primary key values.</param>
    /// <param name="value">Values to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The entity valued task.</returns>
    Task PatchAsync<TUpdate>(string company, IDictionary<string, object?> key, TUpdate value, CancellationToken cancellationToken);

    /// <summary>
    /// Patch a per company entity object.
    /// </summary>
    /// <typeparam name="TUpdate">The type of the t update.</typeparam>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task PatchAsync<TUpdate>(IPerCompanyPrimaryKey key, TUpdate value, CancellationToken cancellationToken);

    /// <summary>
    /// Patch a common entity object.
    /// </summary>
    /// <typeparam name="TUpdate">The type of the t update.</typeparam>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task PatchAsync<TUpdate>(ICommonPrimaryKey key, TUpdate value, CancellationToken cancellationToken);

    /// <summary>
    /// Post a new entity object.
    /// </summary>
    /// <typeparam name="TCreate">Type of the create entity model.</typeparam>
    /// <param name="value">New entity value to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Http response to the post request.</returns>
    Task<TODataCommon> PostAsync<TCreate>(TCreate value, CancellationToken cancellationToken);

    /// <summary>
    /// Post a new entity object.
    /// </summary>
    /// <typeparam name="TCreate">Type of the create entity model.</typeparam>
    /// <param name="company">Company identifier.</param>
    /// <param name="value">New entity value to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Http response to the post request.</returns>
    Task<TODataCommon> PostAsync<TCreate>(string company, TCreate value, CancellationToken cancellationToken);

    /// <summary>
    /// Patch an entity object.
    /// </summary>
    /// <typeparam name="TUpdate">Type of the update entity model.</typeparam>
    /// <param name="company">Company identifier.</param>
    /// <param name="key">Primary key values.</param>
    /// <param name="value">Values to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The response message valued task.</returns>
    Task<HttpResponseMessage> SendPatchAsync<TUpdate>(string company, IDictionary<string, object?> key, TUpdate value, CancellationToken cancellationToken);

    /// <summary>
    /// Patch an entity object.
    /// </summary>
    /// <typeparam name="TUpdate">Type of the update entity model.</typeparam>
    /// <param name="key">Primary key values.</param>
    /// <param name="value">Values to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The response message valued task.</returns>
    Task<HttpResponseMessage> SendPatchAsync<TUpdate>(IDictionary<string, object?> key, TUpdate value, CancellationToken cancellationToken);

    /// <summary>
    /// Post a new entity object.
    /// </summary>
    /// <typeparam name="TCreate">Type of the create entity model.</typeparam>
    /// <param name="value">New entity value to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Http response to the post request.</returns>
    Task<HttpResponseMessage> SendPostAsync<TCreate>(TCreate value, CancellationToken cancellationToken);

    /// <summary>
    /// Post a new entity object.
    /// </summary>
    /// <typeparam name="TCreate">Type of the create entity model.</typeparam>
    /// <param name="company">Company identifier.</param>
    /// <param name="value">New entity value to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Http response to the post request.</returns>
    Task<HttpResponseMessage> SendPostAsync<TCreate>(string company, TCreate value, CancellationToken cancellationToken);
}