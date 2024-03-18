// <copyright file="ExceptionHelperTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Helpers;

using System;

using FluentAssertions;

using Hexalith.Extensions.Helpers;

public class ExceptionHelperTest
{
    [Fact]
    public void GetFullMessageOnNestedExceptionsShouldReturnAllMessages()
    {
        const string msg1 = "Error message 1";
        const string msg2 = "Error 2";
        const string msg3 = "Message :\n Error 3";
        const string msg4 = "Hello 4";
        Exception ex = new(
            msg1,
            new Exception(
                msg2,
                new Exception(
                    msg3,
                    new Exception(msg4))));
        string message = ex.FullMessage();
        _ = message.Should().Be($"{msg1} {msg2} {msg3} {msg4}");
    }
}