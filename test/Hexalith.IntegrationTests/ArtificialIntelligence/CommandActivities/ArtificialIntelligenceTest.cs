// <copyright file="ArtificialIntelligenceTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.IntegrationTests.ArtificialIntelligence.CommandActivities;

using System.Text.Json;
using System.Threading.Tasks;

using FluentAssertions;

public class ArtificialIntelligenceTest
{
    [Fact]
    public Task Should_add_a_country()
    {
        // Arrange
        AddCountryCommand command = new(
            "FR",
            "FRA",
            250,
            "France",
            "Euro",
            "€");
        AddCountryCommand responseCommand = JsonSerializer.Deserialize<AddCountryCommand>(string.Empty);
        _ = responseCommand.Should().BeEquivalentTo(command);
        return Task.CompletedTask;
    }
}