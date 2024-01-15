// ***********************************************************************
// Assembly         : HexalithApplication
// Author           : Jérôme Piquot
// Created          : 01-14-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-14-2024
// ***********************************************************************
// <copyright file="IdentityEmailSender.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace HexalithApplication.Components.Account;

using HexalithApplication.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

// Remove the "else if (EmailSender is IdentityNoOpEmailSender)" block from RegisterConfirmation.razor after updating with a real implementation.

/// <summary>
/// Class IdentityNoOpEmailSender. This class cannot be inherited.
/// Implements the <see cref="Microsoft.AspNetCore.Identity.IEmailSender{HexalithApplication.Data.ApplicationUser}" />.
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Identity.IEmailSender{HexalithApplication.Data.ApplicationUser}" />
internal sealed class IdentityEmailSender : IEmailSender<ApplicationUser>
{
    /// <summary>
    /// The email sender.
    /// </summary>
    private readonly IEmailSender _emailSender;

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityEmailSender" /> class.
    /// </summary>
    /// <param name="emailSender">The email sender.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public IdentityEmailSender(IEmailSender emailSender)
    {
        ArgumentNullException.ThrowIfNull(emailSender);
        _emailSender = emailSender;
    }

    /// <inheritdoc/>
    public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink) =>
        _emailSender.SendEmailAsync(email, "Confirm your email", $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

    /// <inheritdoc/>
    public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode) =>
        _emailSender.SendEmailAsync(email, "Reset your password", $"Please reset your password using the following code: {resetCode}");

    /// <inheritdoc/>
    public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink) =>
        _emailSender.SendEmailAsync(email, "Reset your password", $"Please reset your password by <a href='{resetLink}'>clicking here</a>.");
}