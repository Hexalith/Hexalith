// <copyright file="ErrorMessageTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.Dynamics365Finance.Client;

using FluentAssertions;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Models;

using System.Text.Json;

public class ErrorMessageTest
{
    private readonly string _errorMessageJson =
        $$"""
        {
          "error":{
            "code":"0022",
            "message":"An error has occurred.",
            "innererror":{
              "message":"Infolog: Warning: The sales order header entity does not support updates"
            }
          }
        }
        """;

    // test error message serialization
    [Fact]
    public void TestErrorMessage()
    {
        ErrorResponse? error = JsonSerializer.Deserialize<ErrorResponse>(_errorMessageJson);
        _ = error.Should().NotBeNull();
        _ = error!.Error.Should().NotBeNull();
        _ = error!.Error!.Code.Should().Be("0022");
        _ = error!.Error!.Message.Should().Be("An error has occurred.");
        _ = error!.Error!.InnerError.Should().NotBeNull();
        _ = error!.Error!.InnerError!.Message.Should().Be("Infolog: Warning: The sales order header entity does not support updates");
    }
}
