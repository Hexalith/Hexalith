// <copyright file="ResiliencyPolicyProvider.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Tasks;

/// <summary>
/// Class ResiliencyPolicyProvider.
/// Implements the <see cref="Hexalith.Application.Tasks.IResiliencyPolicyProvider" />.
/// </summary>
/// <seealso cref="Hexalith.Application.Tasks.IResiliencyPolicyProvider" />
public class ResiliencyPolicyProvider : IResiliencyPolicyProvider
{
    /// <inheritdoc/>
    public ResiliencyPolicy GetPolicy(string name)
        => ResiliencyPolicy.CreateDefaultExponentialRetry(); // TODO: Implement policy provider using settings
}