// <copyright file="UpdateSalesOrderHeaderFailedException.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesOrders.Exceptions;

using System;

/// <summary>
/// Class UpdateSalesOrderHeaderFailedException. This class cannot be inherited.
/// Implements the <see cref="Exception" />.
/// </summary>
/// <typeparam name="TUpdate">The type of the t update.</typeparam>
/// <seealso cref="Exception" />
public sealed class UpdateSalesOrderHeaderFailedException<TUpdate> : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSalesOrderHeaderFailedException{TUpdate}"/> class.
    /// </summary>
    public UpdateSalesOrderHeaderFailedException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSalesOrderHeaderFailedException{TUpdate}"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public UpdateSalesOrderHeaderFailedException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSalesOrderHeaderFailedException{TUpdate}"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public UpdateSalesOrderHeaderFailedException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSalesOrderHeaderFailedException{TUpdate}"/> class.
    /// </summary>
    /// <param name="salesId">The sales identifier.</param>
    /// <param name="update">The update.</param>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public UpdateSalesOrderHeaderFailedException(string salesId, TUpdate update, string? message, Exception? innerException)
        : this($"Failed to update {typeof(TUpdate).Name} for sales order '{salesId}'." + message, innerException) => SalesOrderUpdate = update;

    /// <summary>
    /// Gets the update values that failed to be created.
    /// </summary>
    public TUpdate? SalesOrderUpdate { get; }
}