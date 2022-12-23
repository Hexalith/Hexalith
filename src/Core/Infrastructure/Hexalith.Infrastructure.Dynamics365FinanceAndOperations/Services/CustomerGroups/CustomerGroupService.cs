// <copyright file="CustomerGroupService.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Services.CustomerGroups;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;

using Microsoft.Extensions.Logging;

using System.Threading;
using System.Threading.Tasks;

public class CustomerGroupService : Dynamics365FinanceService<CustomerGroup, CustomerGroupCreate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerGroupService"/> class.
    /// </summary>
    /// <param name="client"></param>
    /// <param name="logger"></param>
    public CustomerGroupService(
        IDynamics365FinanceAndOperationsClient<CustomerGroup> client,
        ILogger<CustomerGroupService> logger)
        : base(client, logger)
    {
    }

    public async Task<CustomerGroup> GetAsync(string id, CancellationToken cancellationToken)
    {
        try
        {
            return await Client.GetSingleAsync(
                new Dictionary<string, object>(
                StringComparer.Ordinal)
                {
                     { nameof(CustomerGroup.CustomerGroupId), id },
                },
                cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while getting customer group '{id}'.", ex);
        }
    }
}
