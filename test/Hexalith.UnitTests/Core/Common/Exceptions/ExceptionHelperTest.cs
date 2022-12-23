// <copyright file="ExceptionHelperTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Exceptions;

using FluentAssertions;

using Hexalith.Extensions.Helpers;

using System;

public class ExceptionHelperTest
{
    [Fact]
    public void Retreive_message_from_exception_with_inner_should_succeed()
    {
        Exception ex = new("Test 1", new Exception("Test 2", new Exception("Test 3")));
        _ = ex.FullMessage().Should().Be("Test 1\nTest 2\nTest 3");
    }

    [Fact]
    public void Retreive_message_from_null_exception_should_return_empty()
    {
        const Exception ex = null!;
        _ = ex.FullMessage().Should().BeEmpty();
    }

    [Fact]
    public void Retreive_message_from_simple_exception_should_succeed()
    {
        Exception ex = new("Test");
        _ = ex.FullMessage().Should().Be("Test");
    }
}