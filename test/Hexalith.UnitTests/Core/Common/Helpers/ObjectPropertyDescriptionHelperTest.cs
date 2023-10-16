// ***********************************************************************
// Assembly         : Hexalith.UnitTests
// Author           : Jérôme Piquot
// Created          : 05-14-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-14-2023
// ***********************************************************************
// <copyright file="ObjectPropertyDescriptionHelperTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.UnitTests.Core.Common.Helpers;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using FluentAssertions;

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
        _ = description.Should().Be("My value");
        _ = displayName.Should().Be("My value");
        _ = defaultValue.Should().Be("this value is for me");
        _ = isRequired.Should().BeFalse();
    }

    /// <summary>
    /// Defines the test method Object_property_with_description_attribute_should_return_defined_value.
    /// </summary>
    [Fact]
    public void ObjectPropertyWithDescriptionAttributeShouldReturnDefinedValue()
    {
        IDictionary<string, (string DisplayName, string Description, object DefaultValue, bool IsRequired)> props = ObjectDescriptionHelper.DescribeProperties(typeof(DescriptionPropertyAttributeTest));
        (string displayName, string description, object defaultValue, bool isRequired) = props[nameof(DescriptionPropertyAttributeTest.MyValue)];
        _ = description.Should().Be("This class is used to test a class property with a description attribute");
        _ = displayName.Should().Be("My value");
        _ = defaultValue.Should().BeNull();
        _ = isRequired.Should().BeFalse();
    }

    [Fact]
    public void ObjectPropertyWithDisplayAttributeShouldReturnDefinedValue()
    {
        IDictionary<string, (string DisplayName, string Description, object DefaultValue, bool IsRequired)> props = ObjectDescriptionHelper.DescribeProperties(typeof(DisplayPropertyAttributeTest));
        (string displayName, string description, object defaultValue, bool isRequired) = props[nameof(DisplayPropertyAttributeTest.MyValue)];
        _ = description.Should().Be("This class is used to test a class property with a description attribute");
        _ = displayName.Should().Be("My property value");
        _ = defaultValue.Should().BeNull();
        _ = isRequired.Should().BeFalse();
    }

    [Fact]
    public void ObjectPropertyWithDisplayNameAttributeShouldReturnDefinedValue()
    {
        IDictionary<string, (string DisplayName, string Description, object DefaultValue, bool IsRequired)> props = ObjectDescriptionHelper.DescribeProperties(typeof(DisplayNamePropertyAttributeTest));
        (string displayName, string description, object defaultValue, bool isRequired) = props[nameof(DisplayNamePropertyAttributeTest.MyValue)];
        _ = description.Should().Be("My property value");
        _ = displayName.Should().Be("My property value");
        _ = defaultValue.Should().BeNull();
        _ = isRequired.Should().BeFalse();
    }

    [Fact]
    public void ObjectPropertyWithRequiredValueAttributeShouldReturnDefinedValue()
    {
        IDictionary<string, (string DisplayName, string Description, object DefaultValue, bool IsRequired)> props = ObjectDescriptionHelper.DescribeProperties(typeof(RequiredValuePropertyAttributeTest));
        (string displayName, string description, object defaultValue, bool isRequired) = props[nameof(RequiredValuePropertyAttributeTest.MyValue)];
        _ = description.Should().Be("My value");
        _ = displayName.Should().Be("My value");
        _ = defaultValue.Should().BeNull();
        _ = isRequired.Should().BeTrue();
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