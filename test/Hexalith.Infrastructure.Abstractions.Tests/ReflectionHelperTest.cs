namespace Hexalith.Infrastructure.Abstractions.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;

    using Hexalith.Infrastructure.Abstractions.Tests.Fixtures;
    using Hexalith.Infrastructure.Helpers;

    using FluentAssertions;

    using Xunit;

    public class ReflectionHelperTest
    {
        [Fact]
        public void GetPropertyNotNullValues_should_not_return_null_values()
        {
            var properties = new DummyObject().GetPropertyNotNullValues();
            properties.Should().NotContainKey(nameof(DummyObject.ANullString));
        }

        [Fact]
        public void GetPropertyValues_should_contain_all_properties()
        {
            var properties = new DummyObject().GetPropertyValues()
                .Select(p => p.Key)
                .ToList();
            properties.Should().BeEquivalentTo(new string[]
            {
                nameof(DummyObject.ANullString),
                nameof(DummyObject.AString),
                nameof(DummyObject.ADateTime),
                nameof(DummyObject.ADateTimeOffset),
                nameof(DummyObject.ADecimal),
                nameof(DummyObject.ADouble),
                nameof(DummyObject.AnInteger),
                nameof(DummyObject.AStringArray)
            });
        }

        [Fact]
        public void GetPropertyValues_should_not_return_private_properties()
        {
            var properties = new DummyObject().GetPropertyValues();
            properties.Should().NotContain("_aPrivateFieldNullString");
            properties.Should().NotContain("_aPrivateFieldString");
            properties.Should().NotContain("APrivateString");
        }

        [Fact]
        public void ToDynamic_empty_array()
        {
            Dictionary<string, object> dict = new();
            dynamic dyn = dict.ToDynamic();
            ((IEnumerable)dyn).Should().HaveCount(0);
        }

        [Fact]
        public void ToDynamic_nested_object()
        {
            Dictionary<string, object> dict = new();
            dict.Add("IsInt", 10);
            dict.Add("IsBool", true);
            dict.Add("IsString", "Hello you");
            dict.Add("IsDouble", 22.69d);
            dict.Add("IsDecimal", 11.35m);
            var date = DateTime.Now;
            dict.Add("IsDateTime", date);
            Dictionary<string, object> dict2 = new();
            dict.Add("Dict2", dict2);
            dict2.Add("Val1", 1001);
            Dictionary<string, object> dict3 = new();
            dict2.Add("Dict3", dict3);
            dict3.Add("Val2", 1002);
            dict3.Add("Val3", 1003);
            Dictionary<string, object> dict4 = new();
            dict2.Add("Dict4", dict4);
            dict4.Add("Val4", 1004);
            dict4.Add("Val5", 1005);
            dict4.Add("Val6", 1006);

            dynamic dyn = dict.ToDynamic();
            ((int)dyn.IsInt).Should().Be(10);
            ((bool)dyn.IsBool).Should().Be(true);
            ((string)dyn.IsString).Should().Be("Hello you");
            ((double)dyn.IsDouble).Should().Be(22.69d);
            ((decimal)dyn.IsDecimal).Should().Be(11.35m);
            ((DateTime)dyn.IsDateTime).Should().Be(date);
            ((int)dyn.Dict2.Val1).Should().Be(1001);
            ((int)dyn.Dict2.Dict3.Val2).Should().Be(1002);
            ((int)dyn.Dict2.Dict3.Val3).Should().Be(1003);
            ((int)dyn.Dict2.Dict4.Val4).Should().Be(1004);
            ((int)dyn.Dict2.Dict4.Val5).Should().Be(1005);
            ((int)dyn.Dict2.Dict4.Val6).Should().Be(1006);
        }

        [Fact]
        public void ToDynamic_nested_object_seialization()
        {
            Dictionary<string, object> dict = new();
            dict.Add("IsInt", 10);
            dict.Add("IsBool", true);
            dict.Add("IsString", "Hello you");
            dict.Add("IsDouble", 22.69d);
            dict.Add("IsDecimal", 11.35m);
            var date = DateTime.Now;
            dict.Add("IsDateTime", date);
            Dictionary<string, object> dict2 = new();
            dict.Add("Dict2", dict2);
            dict2.Add("Val1", 1001);
            Dictionary<string, object> dict3 = new();
            dict2.Add("Dict3", dict3);
            dict3.Add("Val2", 1002);
            dict3.Add("Val3", 1003);
            Dictionary<string, object> dict4 = new();
            dict2.Add("Dict4", dict4);
            dict4.Add("Val4", 1004);
            dict4.Add("Val5", 1005);
            dict4.Add("Val6", 1006);

            dynamic dyn = dict.ToDynamic();
            JsonSerializer.Serialize(dict).Should().Be(JsonSerializer.Serialize(dyn));
        }

        [Fact]
        public void ToDynamic_simple_object()
        {
            Dictionary<string, object> dict = new();
            dict.Add("IsInt", 10);
            dict.Add("IsBool", true);
            dict.Add("IsString", "Hello you");
            dict.Add("IsDouble", 22.69d);
            dict.Add("IsDecimal", 11.35m);
            var date = DateTime.Now;
            dict.Add("IsDateTime", date);
            dynamic dyn = dict.ToDynamic();
            ((int)dyn.IsInt).Should().Be(10);
            ((bool)dyn.IsBool).Should().Be(true);
            ((string)dyn.IsString).Should().Be("Hello you");
            ((double)dyn.IsDouble).Should().Be(22.69d);
            ((decimal)dyn.IsDecimal).Should().Be(11.35m);
            ((DateTime)dyn.IsDateTime).Should().Be(date);
        }
    }
}