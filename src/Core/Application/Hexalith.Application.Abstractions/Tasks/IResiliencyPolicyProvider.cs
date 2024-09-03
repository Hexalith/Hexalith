// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-06-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-06-2024
// ***********************************************************************
// <copyright file="IResiliencyPolicyProvider.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

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