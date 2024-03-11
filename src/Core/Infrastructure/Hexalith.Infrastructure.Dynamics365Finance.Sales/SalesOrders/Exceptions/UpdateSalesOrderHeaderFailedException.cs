// <copyright file="UpdateSalesOrderHeaderFailedException.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesOrders.Exceptions;

using System;
using System.Runtime.Serialization;

/// <summary>
/// Exception thrown when creating a sales order header failed.
/// </summary>

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
    /// <param name="update">The sales order that failed to create.</param>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">Inner exception.</param>
    public UpdateSalesOrderHeaderFailedException(string salesId, TUpdate update, string? message, Exception? innerException)
        : this($"Failed to update {typeof(TUpdate).Name} for sales order '{salesId}'." + message, innerException)
    {
        SalesOrderUpdate = update;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSalesOrderHeaderFailedException{TUpdate}"/> class.
    /// </summary>
    /// <param name="info">Serialization information.</param>
    /// <param name="context">Streaming context.</param>
    private UpdateSalesOrderHeaderFailedException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    /// <summary>
    /// Gets the update values that failed to be created.
    /// </summary>
    public TUpdate? SalesOrderUpdate { get; }
}