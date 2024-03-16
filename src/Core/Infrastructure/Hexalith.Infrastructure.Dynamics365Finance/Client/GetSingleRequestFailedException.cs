// <copyright file="GetSingleRequestFailedException.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

/*
 * <Your-Product-Name>
 * Copyright (c) <Year-From>-<Year-To> <Your-Company-Name>
 *
 * Please configure this header in your SonarCloud/SonarQube quality profile.
 * You can also set it in SonarLint.xml additional file for SonarLint or standalone NuGet analyzer.
 */

namespace Hexalith.Infrastructure.Dynamics365Finance.Client;

using Hexalith.Extensions.Common;
using Hexalith.Extensions.Errors;
using Hexalith.Extensions.Helpers;

/// <summary>
/// Exception thrown when a single request failed.
/// </summary>
/// <typeparam name="T">Type of the returned entity.</typeparam>
public sealed class GetSingleRequestFailedException<T> : ApplicationErrorException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetSingleRequestFailedException{T}"/> class.
    /// </summary>
    public GetSingleRequestFailedException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSingleRequestFailedException{T}"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public GetSingleRequestFailedException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSingleRequestFailedException{T}"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public GetSingleRequestFailedException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSingleRequestFailedException{T}"/> class.
    /// </summary>
    /// <param name="entityName">The entity name.</param>
    /// <param name="keys">The keys used to get the entity.</param>
    /// <param name="responseContent">The returned response.</param>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public GetSingleRequestFailedException(string entityName, IDictionary<string, object?> keys, string? responseContent, string? message, Exception? innerException)
        : base(CreateError(entityName, keys, responseContent ?? string.Empty, message ?? string.Empty, innerException), innerException)
    {
        EntityName = entityName;
        Keys = keys;
        ResponseContent = responseContent;
    }

    /// <summary>
    /// Gets or sets the entity name.
    /// </summary>
    /// <value>
    /// The entity name.
    /// </value>
    public string? EntityName { get; set; }

    /// <summary>
    /// Gets or sets the keys used to get the entity.
    /// </summary>
    /// <value>
    /// The keys used to get the entity.
    /// </value>
    public IDictionary<string, object?> Keys { get; set; }

    /// <summary>
    /// Gets or sets the returned response.
    /// </summary>
    /// <value>
    /// The returned response.
    /// </value>
    public string? ResponseContent { get; set; }

    private static ApplicationError CreateError(string entityName, IDictionary<string, object?> keys, string responseContent, string message, Exception? innerException)
    {
        return new ApplicationError
        {
            Title = "Failed to retrieve " + entityName,
            Detail = "Failed to retrieve {Name} with keys [{EntityKeys}] on entity {EntityName}. {Message}",
            Arguments =
            [
                typeof(T).Name,
                string.Join(
                    ';',
                    keys.Select(s => $"{s.Key}='{s.Value}'")),
                entityName,
                message ?? string.Empty,
            ],
            TechnicalDetail = "Response content:\n{ResponseContent}\nError message:\n{ErrorMessage}",
            TechnicalArguments = [responseContent ?? string.Empty, innerException?.FullMessage() ?? string.Empty],
            Category = ErrorCategory.Unknown,
            Type = "Dynamics365Finance",
        };
    }
}