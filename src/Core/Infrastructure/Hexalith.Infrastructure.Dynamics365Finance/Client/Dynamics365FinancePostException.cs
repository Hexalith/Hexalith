// <copyright file="Dynamics365FinancePostException.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Client;

using System;
using System.Runtime.Serialization;
using System.Text.Json;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Class Dynamics365FinancePostException.
/// Implements the <see cref="Exception" />.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TCreate">The type of the create.</typeparam>
/// <seealso cref="Exception" />
[DataContract]
public class Dynamics365FinancePostException<TEntity, TCreate> : Exception
    where TEntity : class, IODataCommon
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinancePostException{TEntity, TCreate}"/> class.
    /// </summary>
    public Dynamics365FinancePostException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinancePostException{TEntity, TCreate}"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public Dynamics365FinancePostException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinancePostException{TEntity, TCreate}"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
    public Dynamics365FinancePostException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinancePostException{TEntity, TCreate}"/> class.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <param name="company">The company.</param>
    /// <param name="value">The value.</param>
    /// <param name="error">The error.</param>
    /// <param name="message">The message.</param>
    /// <param name="responseContent">Content of the response.</param>
    /// <param name="innerException">The inner exception.</param>
    public Dynamics365FinancePostException(Uri? url, string company, TCreate? value, ErrorResponse? error, string? message, string? responseContent, Exception? innerException)
        : base(
            CreateMessage(url, company, value, error, message, responseContent),
            innerException)
    {
        Url = url;
        Company = company;
        Value = value;
        ResponseContent = responseContent;
        Error = error;
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
    public ErrorResponse? Error { get; private set; }

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
    public TCreate? Value { get; }

    private static string CreateMessage(Uri? url, string company, TCreate? value, ErrorResponse? error, string? message, string? responseContent)
    {
        string msg = $"Error while posting {typeof(TEntity).Name} to company {company}. Url: {url?.AbsoluteUri}.";
        if (!string.IsNullOrWhiteSpace(message))
        {
            msg += "\n" + message;
        }

        msg += error != null && error.Error != null && !string.IsNullOrWhiteSpace(error.Error.Message) ? $"\n{error.Error.Message}\n{error.Error.InnerError?.Message}\n{error.Error.InnerError?.InternalException?.Message}" : $"\nResponse content:\n{responseContent}";

        if (!object.Equals(value, default(TCreate)))
        {
            string v = JsonSerializer.Serialize(value, new JsonSerializerOptions { WriteIndented = true });
            if (!string.IsNullOrWhiteSpace(v))
            {
                msg += $"\nValue:\n{v}";
            }
        }

        return msg;
    }
}