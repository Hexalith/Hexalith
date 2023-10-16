// <copyright file="CommandPromptGenerationTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.Prompts;

using FluentAssertions;

using Hexalith.Extensions.Helpers;

public class CommandPromptGenerationTest
{
    [Fact]
    public async Task GenerateCommandPromptShouldSucceed()
    {
        const string assistantEmail = "testai@hexalith.com";
        const string assistantName = "Testai";
        const string userEmail = "johndoe@hexalith.com";
        const string userName = "John Doe";
        string correlationId = UniqueIdHelper.GenerateUniqueStringId();
        AddCountryCommand command = ExampleHelper.CreateExample<AddCountryCommand>();
        CommandPromptGenerator generator = new();
        string prompt = await generator.GeneratePromptAsync<AddCountryCommand>(
            assistantEmail,
            assistantName,
            userEmail,
            userName,
            "en-US",
            correlationId);

        // Assert
        _ = prompt.Should().NotBeNullOrEmpty();
        _ = prompt.Should().Contain(correlationId);
        _ = prompt.Should().Contain(assistantEmail);
        _ = prompt.Should().Contain(assistantName);
        _ = prompt.Should().Contain(userEmail);
        _ = prompt.Should().Contain(userName);
        _ = prompt.Should().Contain(command.AggregateName);
        _ = prompt.Should().Contain(nameof(AddCountryCommand.Name));
        _ = prompt.Should().Contain(nameof(AddCountryCommand.IsoNumber));
        _ = prompt.Should().Contain(nameof(AddCountryCommand.CurrencyName));
        _ = prompt.Should().Contain(nameof(AddCountryCommand.CurrencySymbol));
        _ = prompt.Should().Contain(nameof(AddCountryCommand.Iso2));
        _ = prompt.Should().Contain(nameof(AddCountryCommand.Iso3));
        _ = prompt.Should().Contain(nameof(AddCountryCommand.IsoNumber));
    }
}