// <copyright file="SaveHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components.Pages;

using System;
using System.Security.Claims;

/// <summary>
/// Provides helper methods for saving data.
/// </summary>
public static class SaveHelper
{
    /// <summary>
    /// Validates the save operation for the specified data.
    /// </summary>
    /// <typeparam name="T">The type of the data to validate.</typeparam>
    /// <param name="data">The data to validate.</param>
    /// <param name="user">The user performing the save operation.</param>
    /// <param name="validate">The validation function to apply to the data.</param>
    /// <returns>A <see cref="SaveResult"/> indicating the result of the validation.</returns>
    public static SaveResult ValidateSave<T>(this T? data, ClaimsPrincipal? user, Func<T, bool> validate)
        where T : class, IEntityViewModel
    {
        ArgumentNullException.ThrowIfNull(validate);

        return data is null
            ? SaveResult.InternalError
            : user is null || string.IsNullOrWhiteSpace(user.Identity?.Name)
            ? SaveResult.Unauthorized
            : !data.HasChanges ? SaveResult.NoChangesToApply : !validate(data) ? SaveResult.ValidationErrors : SaveResult.Success;
    }
}