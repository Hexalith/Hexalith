// <copyright file="SessionConstants.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Sessions;

using Hexalith.Application.Sessions.Models;

public static class SessionConstants
{
    public const string IdentityProviderClaimType = "idp";
    public const string SessionExpirationName = $"{nameof(Session)}{nameof(Session.Expiration)}";
    public const string SessionIdName = $"{nameof(Session)}{nameof(Session.Id)}";
}