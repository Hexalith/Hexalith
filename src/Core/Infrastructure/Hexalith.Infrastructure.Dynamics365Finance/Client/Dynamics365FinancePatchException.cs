// <copyright file="Dynamics365FinancePatchException.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Client;

using System;
using System.Runtime.Serialization;
using System.Text.Json;

using Hexalith.Extensions.Common;
using Hexalith.Extensions.Errors;
using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Class Dynamics365FinancePatchException.
/// Implements the <see cref="Exception" />.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TUpdate">The type of the create.</typeparam>
/// <seealso cref="Exception" />
[DataContract]
public class Dynamics365FinancePatchException<TEntity, TUpdate> : ApplicationErrorException
    where TEntity : class, IODataCommon
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinancePatchException{TEntity, TUpdate}"/> class.
    /// </summary>
    public Dynamics365FinancePatchException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinancePatchException{TEntity, TUpdate}"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public Dynamics365FinancePatchException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinancePatchException{TEntity, TUpdate}"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
    public Dynamics365FinancePatchException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinancePatchException{TEntity, TUpdate}"/> class.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <param name="company">The company.</param>
    /// <param name="value">The value.</param>
    /// <param name="error">The error.</param>
    /// <param name="message">The message.</param>
    /// <param name="responseContent">Content of the response.</param>
    /// <param name="innerException">The inner exception.</param>
    public Dynamics365FinancePatchException(
        Uri url,
        string company,
        TUpdate? value,
        ErrorResponse? error,
        string? message,
        string? responseContent,
        Exception? innerException)
        : base(
            CreateMessage(url, company, value, error, message, responseContent),
            innerException)
    {
        Url = url;
        Company = company;
        Value = value;
        ResponseContent = responseContent;
        Dynamics365Error = error;
    }

    /// <summary>
    /// Gets the company.
    /// </summary>
    /// <value>The company.</value>
    public string? Company { get; private set; }

    /// <summary>
    /// Gets the content of the response.
    /// </summary>
    /// <value>The content of the response.</value>
    public ErrorResponse? Dynamics365Error { get; private set; }

    /// <summary>
    /// Gets the content of the response.
    /// </summary>
    /// <value>The content of the response.</value>
    public string? ResponseContent { get; private set; }

    /// <summary>Gets the URL.</summary>
    /// <value>The URL.</value>
    public Uri? Url { get; private set; }

    /// <summary>
    /// Gets the value to create.
    /// </summary>
    /// <value>The value.</value>
    public TUpdate? Value { get; private set; }

    private static ApplicationError CreateMessage(Uri url, string company, TUpdate? value, ErrorResponse? error, string? message, string? responseContent)
    {
        string errorMessage = message + error?.Error?.Message ?? string.Empty;
        InnerErrorMessage? err = error?.Error?.InnerError;
        while (err != null)
        {
            errorMessage += err.Message;
            err = err.InternalException;
        }

        return new ApplicationError
        {
            Title = $"Error while updating {typeof(TEntity).Name} to company {company}.",
            Detail = string.IsNullOrWhiteSpace(errorMessage)
                ? message
                : string.IsNullOrWhiteSpace(message)
                    ? errorMessage
                    : message + "\n" + errorMessage,
            TechnicalDetail = "Error while Patching entity {Entity} to company {Company}. Message: {Message} {ErrorMessage}\nInnerError : {InnerError}\nCode : {ErrorCode}\nUrl : {Url}.\nPatched value : {Body}\nResponseContent : \n{ResponseContent}",
            TechnicalArguments = [
                typeof(TEntity).Name,
                company,
                message ?? string.Empty,
                errorMessage,
                error?.Error?.InnerError?.Message ?? string.Empty,
                error?.Error?.Code ?? string.Empty,
                url.AbsoluteUri,
                (value == null) ? string.Empty : JsonSerializer.Serialize(value, new JsonSerializerOptions { WriteIndented = true }),
                responseContent ?? string.Empty],
            Type = "Interface",
            Category = ErrorCategory.Unknown,
        };
    }
}