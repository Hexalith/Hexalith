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
        await SendEmailAsync(email, "Confirm your email", $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.").ConfigureAwait(false);
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
        await SendEmailAsync(email, "Reset your password", $"Please reset your password using the following code: {resetCode}").ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(resetLink);
        await SendEmailAsync(email, "Reset your password", $"Please reset your password by <a href='{resetLink}'>clicking here</a>.").ConfigureAwait(false);
    }
}