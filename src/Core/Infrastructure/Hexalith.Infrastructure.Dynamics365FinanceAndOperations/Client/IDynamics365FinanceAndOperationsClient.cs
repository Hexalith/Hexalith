// <copyright file="IDynamics365FinanceAndOperationsClient.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Models;

/// <summary>
/// Dynamics 365 Finance and Operations client interface.
/// </summary>
/// <typeparam name="TODataElement">The entity record type.</typeparam>
public interface IDynamics365FinanceAndOperationsClient<TODataElement>
    where TODataElement : IODataElement
{
    /// <summary>
    /// Gets the connection default company.
    /// </summary>
    /// <value>
    /// The connection default company.
    /// </value>
    string DefaultCompany { get; }

    /// <summary>
    /// Execute an action on a Dynamics 365 Finance and Operations entity.
    /// </summary>
    /// <param name="action">Action name.</param>
    /// <param name="parameters">Action parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task.</returns>
    Task DoActionAsync(string action, IDictionary<string, object> parameters, CancellationToken cancellationToken);

    /// <summary>
    /// Get a filtered entity object.
    /// </summary>
    /// <param name="company">Company identifier.</param>
    /// <param name="filter">Filter values.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of entity objects.</returns>
    Task<IEnumerable<TODataElement>> GetAsync(string company, IDictionary<string, object> filter, CancellationToken cancellationToken);

    /// <summary>
    /// Get a filtered entity object list.
    /// </summary>
    /// <typeparam name="T">Type of the entity model.</typeparam>
    /// <param name="filter">Filter values.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of entity objects.</returns>
    Task<IEnumerable<TODataElement>> GetAsync(IDictionary<string, object> filter, CancellationToken cancellationToken);

    /// <summary>
    /// Get entity object by it's primary key.
    /// </summary>
    /// <typeparam name="T">Type of the read entity model.</typeparam>
    /// <param name="keys">Primary key values.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Entity object.</returns>
    Task<TODataElement> GetSingleAsync(IDictionary<string, object> keys, CancellationToken cancellationToken);

    /// <summary>
    /// Get entity object by it's primary key.
    /// </summary>
    /// <typeparam name="T">Type of the read entity model.</typeparam>
    /// <param name="company">Company identifier.</param>
    /// <param name="keys">Primary key values.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Entity object.</returns>
    Task<TODataElement> GetSingleAsync(string company, IDictionary<string, object> keys, CancellationToken cancellationToken);

    /// <summary>
    /// Patch an entity object.
    /// </summary>
    /// <typeparam name="TUpdate">Type of the update entity model.</typeparam>
    /// <param name="key">Primary key values.</param>
    /// <param name="value">Values to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The entity valued task.</returns>
    Task<TODataElement> PatchAsync<TUpdate>(IDictionary<string, object> key, TUpdate value, CancellationToken cancellationToken);

    /// <summary>
    /// Patch an entity object.
    /// </summary>
    /// <typeparam name="TUpdate">Type of the update entity model.</typeparam>
    /// <param name="company">Company identifier.</param>
    /// <param name="key">Primary key values.</param>
    /// <param name="value">Values to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The entity valued task.</returns>
    Task<TODataElement> PatchAsync<TUpdate>(string company, IDictionary<string, object> key, TUpdate value, CancellationToken cancellationToken);

    /// <summary>
    /// Post a new entity object.
    /// </summary>
    /// <typeparam name="TCreate">Type of the create entity model.</typeparam>
    /// <param name="value">New entity value to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Http response to the post request.</returns>
    Task<TODataElement> PostAsync<TCreate>(TCreate value, CancellationToken cancellationToken);

    /// <summary>
    /// Post a new entity object.
    /// </summary>
    /// <typeparam name="TCreate">Type of the create entity model.</typeparam>
    /// <param name="company">Company identifier.</param>
    /// <param name="value">New entity value to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Http response to the post request.</returns>
    Task<TODataElement> PostAsync<TCreate>(string company, TCreate value, CancellationToken cancellationToken);

    /// <summary>
    /// Patch an entity object.
    /// </summary>
    /// <typeparam name="TUpdate">Type of the update entity model.</typeparam>
    /// <param name="company">Company identifier.</param>
    /// <param name="key">Primary key values.</param>
    /// <param name="value">Values to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The response message valued task.</returns>
    Task<HttpResponseMessage> SendPatchAsync<TUpdate>(string company, IDictionary<string, object> key, TUpdate value, CancellationToken cancellationToken);

    /// <summary>
    /// Patch an entity object.
    /// </summary>
    /// <typeparam name="TUpdate">Type of the update entity model.</typeparam>
    /// <param name="key">Primary key values.</param>
    /// <param name="value">Values to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The response message valued task.</returns>
    Task<HttpResponseMessage> SendPatchAsync<TUpdate>(IDictionary<string, object> key, TUpdate value, CancellationToken cancellationToken);

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