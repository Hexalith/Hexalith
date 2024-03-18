// <copyright file="ExceptionHelperTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Exceptions;

using System;

using FluentAssertions;

using Hexalith.Extensions.Helpers;

public class ExceptionHelperTest
{
    [Fact]
    public void RetreiveMessageFromExceptionWithInnerShouldSucceed()
    {
        Exception ex = new("Test 1", new Exception("Test 2", new Exception("Test 3")));
        _ = ex.FullMessage().Should().Be("Test 1 Test 2 Test 3");
    }

    [Fact]
    public void RetreiveMessageFromNullExceptionShouldReturnEmpty()
    {
        const Exception ex = null!;
        _ = ex.FullMessage().Should().BeEmpty();
    }

    [Fact]
    public void RetreiveMessageFromSimpleExceptionShouldSucceed()
    {
        Exception ex = new("Test");
        _ = ex.FullMessage().Should().Be("Test");
    }
}