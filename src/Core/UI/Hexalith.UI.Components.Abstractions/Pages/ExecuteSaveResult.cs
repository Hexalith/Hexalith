// <copyright file="ExecuteSaveResult.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components.Pages;

/// <summary>
/// Represents the result of an execute save operation.
/// </summary>
public enum ExecuteSaveResult
{
    /// <summary>
    /// The operation was successful.
    /// </summary>
    Success,

    /// <summary>
    /// There were no changes to apply.
    /// </summary>
    NoChangesToApply,

    /// <summary>
    /// There were validation errors.
    /// </summary>
    ValidationFailed,

    /// <summary>
    /// A custom error occurred.
    /// </summary>
    CustomError,

    /// <summary>
    /// The user is not authorized to perform the operation.
    /// </summary>
    Unauthorized,

    /// <summary>
    /// An internal error occurred.
    /// </summary>
    InternalError,
}