// <copyright file="IEmailService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Emails;

using System.Threading.Tasks;

/// <summary>
/// Interface IEmailService.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends the asynchronous.
    /// </summary>
    /// <param name="fromEmail">From email.</param>
    /// <param name="fromName">From name.</param>
    /// <param name="toEmail">To email.</param>
    /// <param name="subject">The subject.</param>
    /// <param name="plainTextContent">Content of the plain text.</param>
    /// <param name="htmlContent">Content of the HTML.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task SendAsync(string fromEmail, string fromName, string toEmail, string subject, string? plainTextContent, string? htmlContent, CancellationToken cancellationToken);

    /// <summary>
    /// Sends the asynchronous.
    /// </summary>
    /// <param name="toEmail">To email.</param>
    /// <param name="subject">The subject.</param>
    /// <param name="plainTextContent">Content of the plain text.</param>
    /// <param name="htmlContent">Content of the HTML.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task SendAsync(string toEmail, string subject, string? plainTextContent, string? htmlContent, CancellationToken cancellationToken);
}