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

public class Dynamics365FinanceService<TEntity, TCreate>
    where TEntity : class, IODataElement
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceService{TEntity, TCreate}"/> class.
    /// </summary>
    /// <param name="client"></param>
    /// <param name="logger"></param>
    protected Dynamics365FinanceService(
        IDynamics365FinanceAndOperationsClient<TEntity> client,
        ILogger logger)
    {
        Client = client;
        Logger = logger;
    }

    protected IDynamics365FinanceAndOperationsClient<TEntity> Client { get; }

    protected ILogger Logger { get; }

    public async Task<TEntity> InsertAsync(TCreate entity, CancellationToken cancellationToken)
    {
        try
        {
            TEntity? result = await Client.PostAsync(
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
