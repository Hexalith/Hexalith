// <copyright file="UniqueHelperTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Helpers;

using FluentAssertions;

using Hexalith.Extensions.Helpers;

public class UniqueHelperTest
{
    [Fact]
    public async Task GetAHundredConcurrentDateTimeIdStringWithoutAnyDuplicatesAsync()
    {
        List<Task<string>> ids =[];
        for (int i = 0; i < 100; i++)
        {
            ids.Add(Task.Run(UniqueIdHelper.GenerateDateTimeId));
        }

        string[] result = await Task.WhenAll(ids);
        _ = result.Distinct(StringComparer.Ordinal).Count().Should().Be(100);
    }

    [Fact]
    public void GetAHundredDateTimeIdStringWithoutAnyDuplicates()
    {
        List<string> ids =[];
        for (int i = 0; i < 100; i++)
        {
            ids.Add(UniqueIdHelper.GenerateDateTimeId());
        }

        _ = ids.Distinct(StringComparer.Ordinal).Count().Should().Be(100);
    }

    [Fact]
    public void GetAThousandUniqueIdStringWithoutAnyDuplicates()
    {
        List<string> ids =[];
        for (int i = 0; i < 1000; i++)
        {
            ids.Add(UniqueIdHelper.GenerateUniqueStringId());
        }

        _ = ids.Distinct(StringComparer.Ordinal).Count().Should().Be(1000);
    }

    [Fact]
    public void GetDateTimeIdStringReturns17Chars()
    {
        string id = UniqueIdHelper.GenerateDateTimeId();
        _ = id.Should().HaveLength(17, "Because the id is a millisecond precision date time string");
    }

    [Fact]
    public void GetUniqueIdStringReturns22Chars()
    {
        string id = UniqueIdHelper.GenerateUniqueStringId();
        _ = id.Should().HaveLength(22, "Because the id is a base64 string of 16 bytes");
    }
}