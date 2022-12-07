// <copyright file="IDynamics365FinanceAndOperationsClient.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;

/// <summary>
/// Dynamics 365 Finance and Operations client interface.
/// </summary>
public interface IDynamics365FinanceAndOperationsClient
{
	/// <summary>
	/// Execute an action on a Dynamics 365 Finance and Operations entity.
	/// </summary>
	/// <param name="entityName">Name of the entity.</param>
	/// <param name="action">Action name.</param>
	/// <param name="parameters">Action parameters.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>Task.</returns>
	Task DoActionAsync(string entityName, string action, IDictionary<string, object> parameters, CancellationToken cancellationToken);

	/// <summary>
	/// Get a filtered entity object list.
	/// </summary>
	/// <typeparam name="T">Type of the entity model.</typeparam>
	/// <param name="entityName">Name of the entity.</param>
	/// <param name="filter">Filter values.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>List of entity objects.</returns>
	Task<IEnumerable<T>> GetAsync<T>(string entityName, IDictionary<string, object> filter, CancellationToken cancellationToken);

	/// <summary>
	/// Get entity object by it's primary key.
	/// </summary>
	/// <typeparam name="T">Type of the read entity model.</typeparam>
	/// <param name="entityName">Name of the entity.</param>
	/// <param name="keys">Primary key values.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>Entity object.</returns>
	Task<T> GetSingleAsync<T>(string entityName, IDictionary<string, object> keys, CancellationToken cancellationToken);

	/// <summary>
	/// Patch an entity object.
	/// </summary>
	/// <typeparam name="TUpdate">Type of the update entity model.</typeparam>
	/// <param name="entityName">Name of the entity.</param>
	/// <param name="key">Primary key values.</param>
	/// <param name="value">Values to update.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>The response message valued task.</returns>
	Task<HttpResponseMessage> PatchAsync<TUpdate>(string entityName, IDictionary<string, object> key, TUpdate value, CancellationToken cancellationToken);

	/// <summary>
	/// Patch an entity object.
	/// </summary>
	/// <typeparam name="TUpdate">Type of the update entity model.</typeparam>
	/// <typeparam name="TEntity">Type of the return entity model.</typeparam>
	/// <param name="entityName">Name of the entity.</param>
	/// <param name="key">Primary key values.</param>
	/// <param name="value">Values to update.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>The entity valued task.</returns>
	Task<TEntity> PatchAsync<TUpdate, TEntity>(string entityName, IDictionary<string, object> key, TUpdate value, CancellationToken cancellationToken);

	/// <summary>
	/// Post a new entity object.
	/// </summary>
	/// <typeparam name="T">Type of the create entity model.</typeparam>
	/// <param name="entityName">Name of the entity.</param>
	/// <param name="value">New entity value to create.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>Http response to the post request.</returns>
	Task<HttpResponseMessage> PostAsync<T>(string entityName, T value, CancellationToken cancellationToken);

	/// <summary>
	/// Post a new entity object.
	/// </summary>
	/// <typeparam name="TCreate">Type of the create entity model.</typeparam>
	/// <typeparam name="TEntity">Type of the read entity model.</typeparam>
	/// <param name="entityName">Name of the entity.</param>
	/// <param name="value">New entity value to create.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>Http response to the post request.</returns>
	Task<TEntity> PostAsync<TCreate, TEntity>(string entityName, TCreate value, CancellationToken cancellationToken);
}