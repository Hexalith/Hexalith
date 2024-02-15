// ***********************************************************************
// Assembly         : HexalithApplication
// Author           : Jérôme Piquot
// Created          : 01-14-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-14-2024
// ***********************************************************************
// <copyright file="EmailSender.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.ClientAppOnServer.Services;

using System.Threading.Tasks;

using Hexalith.Application.Emails;
using Hexalith.Infrastructure.Security.Abstractions.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

/// <summary>
/// Class EmailSender.
/// Implements the <see cref="IEmailSender" />.
/// </summary>
/// <seealso cref="IEmailSender" />
public class EmailSender : IEmailSender<ApplicationUser>, IEmailSender
{
    /// <summary>
    /// The email service.
    /// </summary>
    private readonly IEmailService _emailService;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailSender" /> class.
    /// </summary>
    /// <param name="emailService">The email service.</param>
    /// <exception cref="ArgumentNullException">null.</exception>
    public EmailSender(IEmailService emailService)
    {
        ArgumentNullException.ThrowIfNull(emailService);
        _emailService = emailService;
    }

    /// <inheritdoc/>
    public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(confirmationLink);
        string subject = "Confirm your email";
        string message = $"Please confirm your account by clicking this link: <a href='{confirmationLink}'>link</a>";
        await SendEmailAsync(email, subject, message).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        => await _emailService
        .SendAsync(
            email,
            subject,
            null,
            htmlMessage,
            CancellationToken.None)
        .ConfigureAwait(false);

    /// <inheritdoc/>
    public async Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(resetCode);
        string subject = "Reset password";
        string message = $"Please reset your password by using this code: <b>{resetCode}</b>";

        await SendEmailAsync(email, subject, message).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(resetLink);
        string subject = "Reset password";
        string message = $"Please reset your password by clicking this link: <a href='{resetLink}'>link</a>";
        await SendEmailAsync(email, subject, message).ConfigureAwait(false);
    }
}