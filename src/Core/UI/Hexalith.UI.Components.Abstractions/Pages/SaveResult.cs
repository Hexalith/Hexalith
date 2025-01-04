// <copyright file="SaveResult.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components.Pages;
/// <summary>
/// Represents the result of a save operation.
/// </summary>
/// <param name="Result">The result of the save operation.</param>
/// <param name="Message">The message associated with the save result.</param>
public record SaveResult(ExecuteSaveResult Result, string? Message)
{
    /// <summary>
    /// Gets the success result.
    /// </summary>
    public static SaveResult Success => new(ExecuteSaveResult.Success, null);

    /// <summary>
    /// Gets the unauthorized result.
    /// </summary>
    public static SaveResult Unauthorized => new(ExecuteSaveResult.Unauthorized, null);

    /// <summary>
    /// Gets the internal error result.
    /// </summary>
    public static SaveResult InternalError => new(ExecuteSaveResult.InternalError, null);

    /// <summary>
    /// Gets the result indicating no changes to apply.
    /// </summary>
    public static SaveResult NoChangesToApply => new(ExecuteSaveResult.NoChangesToApply, null);

    /// <summary>
    /// Gets the result indicating validation errors.
    /// </summary>
    public static SaveResult ValidationErrors => new(ExecuteSaveResult.ValidationFailed, null);

    /// <summary>
    /// Creates a custom error result with the specified message.
    /// </summary>
    /// <param name="message">The custom error message.</param>
    /// <returns>A new instance of <see cref="SaveResult"/> with a custom error.</returns>
    public static SaveResult CustomError(string message) => new(ExecuteSaveResult.CustomError, message);
}