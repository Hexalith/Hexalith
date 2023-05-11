// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365FinanceAndOperations
// Author           : Jérôme Piquot
// Created          : 02-05-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-08-2023
// ***********************************************************************
// <copyright file="GetRequestFailedException.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;

using System.Runtime.Serialization;
using System.Text.Json;

/// <summary>
/// Exception thrown when a single request failed.
/// </summary>
/// <typeparam name="T">Type of the returned entity.</typeparam>
[DataContract]
public sealed class GetRequestFailedException<T> : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetRequestFailedException{T}" /> class.
    /// </summary>
    public GetRequestFailedException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetRequestFailedException{T}" /> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public GetRequestFailedException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetRequestFailedException{T}" /> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public GetRequestFailedException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetRequestFailedException{T}" /> class.
    /// </summary>
    /// <param name="entityName">The entity name.</param>
    /// <param name="filter">The filter used to get the entity list.</param>
    /// <param name="responseContent">The returned response.</param>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public GetRequestFailedException(string entityName, IDictionary<string, object?> filter, string? responseContent, string? message, Exception? innerException)
        : base($"Failed to retrieve {typeof(T).Name} with filter {JsonSerializer.Serialize(filter)} on entity {entityName}. " + message, innerException)
    {
        EntityName = entityName;
        Filter = filter;
        ResponseContent = responseContent;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetRequestFailedException{T}"/> class.
    /// </summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
    private GetRequestFailedException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    /// <summary>
    /// Gets the entity name.
    /// </summary>
    /// <value>The entity name.</value>
    public string? EntityName { get; private set; }

    /// <summary>
    /// Gets the keys used to get the entity.
    /// </summary>
    /// <value>The keys used to get the entity.</value>
    public IDictionary<string, object?>? Filter { get; private set; }

    /// <summary>
    /// Gets the returned response.
    /// </summary>
    /// <value>The returned response.</value>
    public string? ResponseContent { get; private set; }

    /// <inheritdoc/>
    public override void GetObjectData(SerializationInfo info, StreamingContext context) => base.GetObjectData(info, context);
}