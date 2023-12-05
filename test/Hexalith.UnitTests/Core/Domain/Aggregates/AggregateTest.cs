// ***********************************************************************
// Assembly         : Hexalith.UnitTests
// Author           : Jérôme Piquot
// Created          : 11-15-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-15-2023
// ***********************************************************************
// <copyright file="AggregateTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.UnitTests.Core.Domain.Aggregates;

using FluentAssertions;

using Hexalith.Domain.Aggregates;

/// <summary>
/// Class AggregateTest.
/// </summary>
public class AggregateTest
{
    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void NormalizeEmptyStringShouldThrowArgumentException(string id)
    {
        // Act
        Action act = () => Aggregate.Normalize(id);

        // Assert
        _ = act.Should().Throw<ArgumentException>();
    }

    // [Theory]
    // [InlineData("a~")]
    // [InlineData("~~~~a")]
    // [InlineData("~")]
    // [InlineData("~ ~~")]
    // public void NormalizeStringContainingReplacementCharacterShouldThrowInvalidOperationException(string id)
    // {
    //    // Act
    //    Action act = () => Aggregate.Normalize(id);

    // // Assert
    //    _ = act.Should().Throw<InvalidOperationException>();
    // }

    // [Theory]
    // [InlineData("a ", "a~")]
    // [InlineData("    a", "~~~~a")]
    // [InlineData("a b", "a~b")]
    // [InlineData("d  e", "d~~e")]
    // public void NormalizeStringContainingSpacesShouldReplaceWhiteSpaces(string id, string normalizedId)
    // {
    //    // Act
    //    string result = Aggregate.Normalize(id);

    // // Assert
    //    _ = result.Should().Be(normalizedId);
    // }
}