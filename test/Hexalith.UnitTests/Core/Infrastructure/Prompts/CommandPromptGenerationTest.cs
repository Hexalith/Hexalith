// <copyright file="CommandPromptGenerationTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.Prompts;

using Hexalith.Commons.UniqueIds;
using Hexalith.Extensions.Helpers;

using Shouldly;

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
        prompt.ShouldNotBeNullOrEmpty();
        prompt.ShouldContain(correlationId);
        prompt.ShouldContain(assistantEmail);
        prompt.ShouldContain(assistantName);
        prompt.ShouldContain(userEmail);
        prompt.ShouldContain(userName);
        prompt.ShouldContain(command.DomainName);
        prompt.ShouldContain(nameof(AddCountryCommand.Name));
        prompt.ShouldContain(nameof(AddCountryCommand.IsoNumber));
        prompt.ShouldContain(nameof(AddCountryCommand.CurrencyName));
        prompt.ShouldContain(nameof(AddCountryCommand.CurrencySymbol));
        prompt.ShouldContain(nameof(AddCountryCommand.Iso2));
        prompt.ShouldContain(nameof(AddCountryCommand.Iso3));
        prompt.ShouldContain(nameof(AddCountryCommand.IsoNumber));
    }
}