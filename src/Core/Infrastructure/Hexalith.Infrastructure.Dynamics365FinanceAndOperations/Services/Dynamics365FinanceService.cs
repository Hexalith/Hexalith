// <copyright file="Dynamics365FinanceService.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Services;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Models;

using Microsoft.Extensions.Logging;

using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Class Dynamics365FinanceService.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TCreate">The type of the create.</typeparam>
public class Dynamics365FinanceService<TEntity, TCreate>
    where TEntity : class, IODataElement
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceService{TEntity, TCreate}" /> class.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="logger">The logger.</param>
    protected Dynamics365FinanceService(
        IDynamics365FinanceAndOperationsClient<TEntity> client,
        ILogger logger)
    {
        Client = client;
        Logger = logger;
    }

    /// <summary>
    /// Gets the client.
    /// </summary>
    /// <value>The client.</value>
    protected IDynamics365FinanceAndOperationsClient<TEntity> Client { get; }

    /// <summary>
    /// Gets the logger.
    /// </summary>
    /// <value>The logger.</value>
    protected ILogger Logger { get; }

    /// <summary>
    /// Insert as an asynchronous operation.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;TEntity&gt; representing the asynchronous operation.</returns>
    /// <exception cref="Dynamics365FinanceInsertException{TEntity, TCreate}"></exception>
    public async Task<TEntity> InsertAsync(TCreate entity, CancellationToken cancellationToken)
    {
        try
        {
            TEntity result = await Client.PostAsync(
                entity,
                cancellationToken).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            throw new Dynamics365FinanceInsertException<TEntity, TCreate>(entity, ex);
        }
    }
}
