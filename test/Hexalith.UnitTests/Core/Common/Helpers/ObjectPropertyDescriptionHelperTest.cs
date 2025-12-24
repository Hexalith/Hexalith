// <copyright file="ObjectPropertyDescriptionHelperTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Helpers;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Shouldly;

using Hexalith.Extensions.Helpers;

/// <summary>
/// Class ObjectDescriptionHelperTest.
/// </summary>
public class ObjectPropertyDescriptionHelperTest
{
    [Fact]
    public void ObjectPropertyWithDefaultValueAttributeShouldReturnDefinedValue()
    {
        IDictionary<string, (string DisplayName, string Description, object DefaultValue, bool IsRequired)> props = ObjectDescriptionHelper.DescribeProperties(typeof(DefaultValuePropertyAttributeTest));
        (string displayName, string description, object defaultValue, bool isRequired) = props[nameof(DefaultValuePropertyAttributeTest.MyValue)];
        description.ShouldBe("My value");
        displayName.ShouldBe("My value");
        defaultValue.ShouldBe("this value is for me");
        isRequired.ShouldBeFalse();
    }

    /// <summary>
    /// Defines the test method Object_property_with_description_attribute_should_return_defined_value.
    /// </summary>
    [Fact]
    public void ObjectPropertyWithDescriptionAttributeShouldReturnDefinedValue()
    {
        IDictionary<string, (string DisplayName, string Description, object DefaultValue, bool IsRequired)> props = ObjectDescriptionHelper.DescribeProperties(typeof(DescriptionPropertyAttributeTest));
        (string displayName, string description, object defaultValue, bool isRequired) = props[nameof(DescriptionPropertyAttributeTest.MyValue)];
        description.ShouldBe("This class is used to test a class property with a description attribute");
        displayName.ShouldBe("My value");
        defaultValue.ShouldBeNull();
        isRequired.ShouldBeFalse();
    }

    [Fact]
    public void ObjectPropertyWithDisplayAttributeShouldReturnDefinedValue()
    {
        IDictionary<string, (string DisplayName, string Description, object DefaultValue, bool IsRequired)> props = ObjectDescriptionHelper.DescribeProperties(typeof(DisplayPropertyAttributeTest));
        (string displayName, string description, object defaultValue, bool isRequired) = props[nameof(DisplayPropertyAttributeTest.MyValue)];
        description.ShouldBe("This class is used to test a class property with a description attribute");
        displayName.ShouldBe("My property value");
        defaultValue.ShouldBeNull();
        isRequired.ShouldBeFalse();
    }

    [Fact]
    public void ObjectPropertyWithDisplayNameAttributeShouldReturnDefinedValue()
    {
        IDictionary<string, (string DisplayName, string Description, object DefaultValue, bool IsRequired)> props = ObjectDescriptionHelper.DescribeProperties(typeof(DisplayNamePropertyAttributeTest));
        (string displayName, string description, object defaultValue, bool isRequired) = props[nameof(DisplayNamePropertyAttributeTest.MyValue)];
        description.ShouldBe("My property value");
        displayName.ShouldBe("My property value");
        defaultValue.ShouldBeNull();
        isRequired.ShouldBeFalse();
    }

    [Fact]
    public void ObjectPropertyWithRequiredValueAttributeShouldReturnDefinedValue()
    {
        IDictionary<string, (string DisplayName, string Description, object DefaultValue, bool IsRequired)> props = ObjectDescriptionHelper.DescribeProperties(typeof(RequiredValuePropertyAttributeTest));
        (string displayName, string description, object defaultValue, bool isRequired) = props[nameof(RequiredValuePropertyAttributeTest.MyValue)];
        description.ShouldBe("My value");
        displayName.ShouldBe("My value");
        defaultValue.ShouldBeNull();
        isRequired.ShouldBeTrue();
    }

    public class DefaultValuePropertyAttributeTest
    {
        [DefaultValue("this value is for me")]
        public string MyValue { get; set; }
    }

    /// <summary>
    /// Class DescriptionPropertyAttributeTest.
    /// </summary>
    public class DescriptionPropertyAttributeTest
    {
        [Description("This class is used to test a class property with a description attribute")]
        public string MyValue { get; set; }
    }

    public class DisplayNamePropertyAttributeTest
    {
        [DisplayName("My property value")]
        public string MyValue { get; set; }
    }

    public class DisplayPropertyAttributeTest
    {
        [Display(Name = "My property value", Description = "This class is used to test a class property with a description attribute")]
        public string MyValue { get; set; }
    }

    public class RequiredValuePropertyAttributeTest
    {
        [Required]
        public string MyValue { get; set; }
    }
}