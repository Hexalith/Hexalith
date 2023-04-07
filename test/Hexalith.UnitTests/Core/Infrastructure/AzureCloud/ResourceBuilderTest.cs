// <copyright file="ResourceBuilderTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.AzureCloud;

using FluentAssertions;

using Hexalith.Infrastructure.AzureCloud.Builders;

using Microsoft.Extensions.Logging;

using Moq;

public class ResourceBuilderTest
{
    [Fact]
    public void Builder_should_contain_all_existing_and_not_existing_processes()
    {
        Mock<ILoggerFactory> loggerFactory = new();
        AzureBuilder builder = new("toto", loggerFactory.Object);
        _ = builder.AddResourceGroup("toto", true);
        _ = builder.OrderedProcesses.Count().Should().Be(4);
    }

    [Fact]
    public void Builder_should_contain_all_processes()
    {
        Mock<ILoggerFactory> loggerFactory = new();
        AzureBuilder builder = new("toto", loggerFactory.Object);
        _ = builder.AddResourceGroup("toto", "toto");
        _ = builder.OrderedProcesses.Count().Should().Be(3);
    }

    [Fact]
    public void Builder_should_contain_all_resources()
    {
        Mock<ILoggerFactory> loggerFactory = new();
        AzureBuilder builder = new("toto", loggerFactory.Object);
        _ = builder.AddResourceGroup("titi", "tata");
        _ = builder.Resources.Count().Should().Be(2);
    }
}