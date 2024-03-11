// ***********************************************************************
// Assembly         : Christofle.Infrastructure.Dynamics365Finance.RestClient
// Author           : Jérôme Piquot
// Created          : 08-09-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-02-2023
// ***********************************************************************
// <copyright file="PackingSlipLineService.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.PackingSlips;
using Hexalith.Infrastructure.Dynamics365Finance.Client;

/// <summary>
/// Class PackingSlipLineService.
/// Implements the <see cref="IPackingSlipService" />.
/// </summary>
/// <seealso cref="IPackingSlipService" />
public class PackingSlipLineService : IPackingSlipService
{
    /// <summary>
    /// The client.
    /// </summary>
    private readonly IDynamics365FinanceClient<StockedPackingSlipLine> _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="PackingSlipLineService" /> class.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public PackingSlipLineService(IDynamics365FinanceClient<StockedPackingSlipLine> client)
    {
        ArgumentNullException.ThrowIfNull(client);
        _client = client;
    }
}