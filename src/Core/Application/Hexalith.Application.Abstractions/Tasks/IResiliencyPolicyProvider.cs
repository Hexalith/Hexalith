// <copyright file="IResiliencyPolicyProvider.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Tasks;

/// <summary>
/// Interface IResiliencyPolicyProvider.
/// </summary>
public interface IResiliencyPolicyProvider
{
    /// <summary>
    /// Gets the policy.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>ResiliencyPolicy.</returns>
    ResiliencyPolicy GetPolicy(string name);
}