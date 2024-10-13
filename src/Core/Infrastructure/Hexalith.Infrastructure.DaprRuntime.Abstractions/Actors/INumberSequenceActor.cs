// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime
// Author           : Jérôme Piquot
// Created          : 10-28-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-28-2023
// ***********************************************************************
// <copyright file="INumberSequenceActor.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Actors;

using Dapr.Actors;

/// <summary>
/// Interface INumberSequenceActor
/// Extends the <see cref="IActor" />.
/// </summary>
/// <seealso cref="IActor" />
public interface INumberSequenceActor : IActor
{
    /// <summary>
    /// Gets the asynchronous.
    /// </summary>
    /// <returns>Task&lt;System.Nullable&lt;System.String&gt;&gt;.</returns>
    Task<long> NextAsync();
}