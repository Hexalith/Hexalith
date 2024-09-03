// ***********************************************************************
// Assembly         : Hexalith.TestMocks
// Author           : Jérôme Piquot
// Created          : 12-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-04-2023
// ***********************************************************************
// <copyright file="AssertionHelper.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.TestMocks;

using System;
using System.Text.Json;

using FluentAssertions;
using FluentAssertions.Primitives;

/// <summary>
/// Class AssertionHelper.
/// </summary>
public static class AssertionHelper
{
    /// <summary>
    /// Bes the json serializable.
    /// </summary>
    /// <param name="assertions">The assertions.</param>
    /// <param name="because">The because.</param>
    /// <param name="becauseArgs">The because arguments.</param>
    /// <returns>AndConstraint&lt;ObjectAssertions&gt;.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static AndConstraint<ObjectAssertions> BeJsonSerializable(
        this ObjectAssertions assertions,
        string because,
        params object[] becauseArgs)
    {
        ArgumentNullException.ThrowIfNull(assertions);
        ArgumentNullException.ThrowIfNull(assertions.Subject);
        string json = JsonSerializer.Serialize(assertions.Subject);
        object? result = JsonSerializer.Deserialize(json, assertions.Subject.GetType());
        _ = result.Should().NotBeNull();
        _ = assertions.Subject.Should().BeEquivalentTo(result, because, becauseArgs);
        return new AndConstraint<ObjectAssertions>(assertions);
    }
}