// <copyright file="IFalseTrueFieldValueDisabledService.cs">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components.Services;

using System.Threading.Tasks;

/// <summary>
/// Represents a service for retrieving the disabled false or true values of a field.
/// </summary>
public interface IFalseTrueFieldValueDisabledService
{
    /// <summary>
    /// Gets the disabled false and true values of a boolean field asynchronously.
    /// </summary>
    /// <param name="fieldId">The field identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the disabled false and true values.</returns>
    Task<(bool FalseDisabled, bool TrueDisabled)> FalseTrueFieldDisabledValuesAsync(string fieldId, CancellationToken cancellationToken);
}