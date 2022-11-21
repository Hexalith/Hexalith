// Fiveforty S.A. Paris France (2022)

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;

using System.Collections.Generic;

/// <summary>
/// Dynamics 365 Finance and Operations client interface.
/// </summary>
public interface IDynamics365FinanceAndOperationsClient
{
	/// <summary>
	/// Execute an action on a Dynamics 365 Finance and Operations entity.
	/// </summary>
	/// <param name="entityName">Name of the entity</param>
	/// <param name="action">Action name</param>
	/// <param name="parameters">Action parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Task</returns>
	Task DoActionAsync(string entityName, string action, Dictionary<string, object> parameters, CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a filtered entity object list
	/// </summary>
	/// <typeparam name="T">Type of the entity model</typeparam>
	/// <param name="entityName">Name of the entity</param>
	/// <param name="filter">Filter values</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of entity objects</returns>
	Task<IEnumerable<T>> GetAsync<T>(string entityName, Dictionary<string, object> filter, CancellationToken cancellationToken = default);

	/// <summary>
	/// Get entity object by it's primary key
	/// </summary>
	/// <typeparam name="T">Type of the read entity model</typeparam>
	/// <param name="entityName">Name of the entity</param>
	/// <param name="key">Primary key values</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Entity object</returns>
	Task<T> GetSingleAsync<T>(string entityName, Dictionary<string, object> keys, CancellationToken cancellationToken = default);

	/// <summary>
	/// Patch an entity object
	/// </summary>
	/// <typeparam name="T">Type of the update entity model</typeparam>
	/// <param name="entityName">Name of the entity</param>
	/// <param name="key">Primary key values</param>
	/// <param name="value">Values to update</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Task</returns>
	Task PatchAsync<T>(string entityName, Dictionary<string, object> key, T value, CancellationToken cancellationToken = default);

	/// <summary>
	/// Post a new entity object
	/// </summary>
	/// <typeparam name="T">Type of the create entity model</typeparam>
	/// <param name="entityName">Name of the entity</param>
	/// <param name="value">New entity value to create</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Http response to the post request</returns>
	Task<HttpResponseMessage> PostAsync<T>(string entityName, T value, CancellationToken cancellationToken = default);

	/// <summary>
	/// Post a new entity object
	/// </summary>
	/// <typeparam name="T">Type of the create entity model</typeparam>
	/// <typeparam name="R">Type of the read entity model</typeparam>
	/// <param name="entityName">Name of the entity</param>
	/// <param name="value">New entity value to create</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Http response to the post request</returns>
	Task<R> PostAsync<T, R>(string entityName, T value, CancellationToken cancellationToken = default);
}