// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance
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

namespace Hexalith.Infrastructure.Dynamics365Finance.Client;

using System.Runtime.Serialization;
using System.Text.Json;

using Hexalith.Extensions.Common;
using Hexalith.Extensions.Errors;
using Hexalith.Extensions.Helpers;

/// <summary>
/// Exception thrown when a single request failed.
/// </summary>
/// <typeparam name="T">Type of the returned entity.</typeparam>
[DataContract]
public sealed class GetRequestFailedException<T> : ApplicationErrorException
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
        : base(
            new ApplicationError
            {
                Title = "Dynamics 365 finance get request failed",
                Detail = "Failed to retrieve {Name} with filter {Filter} on entity {EntityName}. {Message}",
                Category = ErrorCategory.Technical,
                Arguments = new[]
                {
                    typeof(T).Name,
                    JsonSerializer.Serialize(filter) ?? string.Empty,
                    entityName,
                    message ?? string.Empty,
                },
                TechnicalDetail = "{TechnicalMessage}",
                TechnicalArguments = new[] { innerException?.FullMessage() ?? string.Empty },
            },
            innerException)
    {
        EntityName = entityName;
        Filter = filter;
        ResponseContent = responseContent;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetRequestFailedException{T}" /> class.
    /// </summary>
    /// <param name="entityName">Name of the entity.</param>
    /// <param name="url">The URL.</param>
    /// <param name="responseContent">Content of the response.</param>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public GetRequestFailedException(string entityName, Uri url, string? responseContent, string? message, Exception? innerException)
        : base(
            new ApplicationError
            {
                Title = "Dynamics 365 finance get request failed",
                Detail = "Failed to retrieve {Name} on entity {EntityName}. {Message}",
                Category = ErrorCategory.Technical,
                Arguments = new[]
                {
                    typeof(T).Name,
                    url.AbsoluteUri,
                    entityName,
                    message ?? string.Empty,
                },
                TechnicalDetail = "Failed on HTTP GET call. Url : {Url}. {TechnicalMessage}",
                TechnicalArguments = new[] { url.AbsoluteUri, innerException?.FullMessage() ?? string.Empty },
            },
            innerException)
    {
        EntityName = entityName;
        Filter = null;
        ResponseContent = responseContent;
        Url = url;
    }

    /// <summary>
    /// Gets or sets the entity name.
    /// </summary>
    /// <value>The entity name.</value>
    [DataMember]
    public string? EntityName { get; set; }

    /// <summary>
    /// Gets or sets the keys used to get the entity.
    /// </summary>
    /// <value>The keys used to get the entity.</value>
    [DataMember]
    public IDictionary<string, object?>? Filter { get; set; }

    /// <summary>
    /// Gets or sets the returned response.
    /// </summary>
    /// <value>The returned response.</value>
    [DataMember]
    public string? ResponseContent { get; set; }

    /// <summary>
    /// Gets or sets the URL.
    /// </summary>
    /// <value>The URL.</value>
    [DataMember]
    public Uri? Url { get; set; }
}