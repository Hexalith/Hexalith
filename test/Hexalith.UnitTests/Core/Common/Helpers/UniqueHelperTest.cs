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
    public async Task Get_a_hundred_concurrent_date_time_id_string_without_any_duplicatesAsync()
    {
        List<Task<string>> ids = new();
        for (int i = 0; i < 100; i++)
        {
            ids.Add(Task.Run(UniqueIdHelper.GenerateDateTimeId));
        }

        string[] result = await Task.WhenAll(ids);
        _ = result.Distinct(StringComparer.Ordinal).Count().Should().Be(100);
    }

    [Fact]
    public void Get_a_hundred_date_time_id_string_without_any_duplicates()
    {
        List<string> ids = new();
        for (int i = 0; i < 100; i++)
        {
            ids.Add(UniqueIdHelper.GenerateDateTimeId());
        }

        _ = ids.Distinct(StringComparer.Ordinal).Count().Should().Be(100);
    }

    [Fact]
    public void Get_a_thousand_unique_id_string_without_any_duplicates()
    {
        List<string> ids = new();
        for (int i = 0; i < 1000; i++)
        {
            ids.Add(UniqueIdHelper.GenerateUniqueStringId());
        }

        _ = ids.Distinct(StringComparer.Ordinal).Count().Should().Be(1000);
    }

    [Fact]
    public void Get_date_time_id_string_returns_17_chars()
    {
        string id = UniqueIdHelper.GenerateDateTimeId();
        _ = id.Should().HaveLength(17, "Because the id is a millisecond precision date time string");
    }

    [Fact]
    public void Get_unique_id_string_returns_22_chars()
    {
        string id = UniqueIdHelper.GenerateUniqueStringId();
        _ = id.Should().HaveLength(22, "Because the id is a base64 string of 16 bytes");
    }
}