namespace Hexalith.Infrastructure.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text.Json;

    using Hexalith.Infrastructure.Helpers;
    using Hexalith.Infrastructure.Tests.Fixture;

    using FluentAssertions;

    using Microsoft.Extensions.Primitives;

    using Xunit;

    public class ReflectionHelperTest
    {
        [Fact]
        public void Assembly_GetConcreteClasses()
        {
            var result = GetType().Assembly.GetConcreteClasses<ITestInterface>();
            result.Should().HaveCount(6);
            result.Should().Contain(new Type[] { typeof(TestConcrete1), typeof(TestConcrete2), typeof(TestConcrete3), typeof(TestConcrete4), typeof(TestConcrete5), typeof(TestConcrete6) });
        }

        [Fact]
        public void AssemblyGetConcreteClasses_NoClasses()
        {
            var result = GetType().Assembly.GetConcreteClasses<ITestInterfaceNoClass>();
            result.Should().HaveCount(0);
        }

        [Fact]
        public void GetInterfaceConcreteClassTypes()
        {
            var result = ReflectionHelper.GetInterfaceConcreteClassTypes(this.GetType().Assembly, typeof(ITestInterface));
            result.Should().HaveCount(6);
            result.Should().Contain(new Type[] { typeof(TestConcrete1), typeof(TestConcrete2), typeof(TestConcrete3), typeof(TestConcrete4), typeof(TestConcrete5), typeof(TestConcrete6) });
        }

        [Fact]
        public void GetInterfaceConcreteClassTypes_Generic()
        {
            var result = ReflectionHelper.GetInterfaceConcreteClassTypes(this.GetType().Assembly, typeof(ITestInterface<>));
            result.Should().HaveCount(3);
            result.Should().Contain(new Type[] { typeof(TestConcrete4), typeof(TestConcrete5), typeof(TestConcrete6) });
        }

        [Fact]
        public void GetInterfaceConcreteClassTypes_NoClasses()
        {
            var result = ReflectionHelper.GetInterfaceConcreteClassTypes(this.GetType().Assembly, typeof(ITestInterfaceNoClass));
            result.Should().HaveCount(0);
        }

        [Fact]
        public void HasInterface_false()
        {
            typeof(TestConcrete1)
                .HasInterface(typeof(ITestInterfaceNoClass))
                .Should()
                .BeFalse();
        }

        [Fact]
        public void HasInterface_generic()
        {
            typeof(TestConcrete4)
                .HasInterface(typeof(ITestInterface<int>))
                .Should()
                .BeTrue();
            typeof(TestConcrete5)
                .HasInterface(typeof(ITestInterface<string>))
                .Should()
                .BeTrue();
        }

        [Fact]
        public void HasInterface_generic_base()
        {
            typeof(TestConcrete4)
                .HasInterface(typeof(ITestInterface<>))
                .Should()
                .BeTrue();
            typeof(TestConcrete5)
                .HasInterface(typeof(ITestInterface<>))
                .Should()
                .BeTrue();
        }

        [Fact]
        public void HasInterface_NotInterface()
        {
            typeof(TestConcrete1).Invoking(t => t
                .HasInterface(typeof(int)))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void HasInterface_NullArguments()
        {
            Type type = null;
            type.Invoking(t => t
                .HasInterface(typeof(ITestInterface)))
                .Should()
                .Throw<ArgumentNullException>();
            typeof(TestConcrete1).Invoking(t => t
                .HasInterface(null))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void HasInterface_true()
        {
            typeof(TestConcrete1)
                .HasInterface(typeof(ITestInterface))
                .Should()
                .BeTrue();
        }

        [Theory]
        [InlineData("L l")]
        [InlineData("/L l/")]
        [InlineData("\\/L l\\/")]
        [InlineData("àaA")]
        [InlineData("Eéèe")]
        [InlineData("M@fd1523çççç\\\\èé/+ {}")]
        [InlineData("+-*/=")]
        public void ToObjet_set_base_string_property_check(string value)
        {
            var values = new Dictionary<string, StringValues>
            {
                { nameof(DummyBaseObject.StringBaseProperty), JsonSerializer.Serialize(value) }
            };
            var obj = (DummyObject)values.ToObject(typeof(DummyObject));
            obj.StringBaseProperty.Should().Be(value);
        }

        [Theory]
        [InlineData("L l")]
        [InlineData("/L l/")]
        [InlineData("\\/L l\\/")]
        [InlineData("àaA")]
        [InlineData("Eéèe")]
        [InlineData("M@fd1523çççç\\\\èé/+ {}")]
        [InlineData("+-*/=")]
        public void ToObjet_set_init_only_set_string_property_check(string value)
        {
            var values = new Dictionary<string, StringValues>
            {
                { nameof(DummyObject.StringInitOnlySetProperty), JsonSerializer.Serialize(value) }
            };
            var obj = (DummyObject)values.ToObject(typeof(DummyObject));
            obj.StringInitOnlySetProperty.Should().Be(value);
        }

        [Theory]
        [InlineData("L l")]
        [InlineData("/L l/")]
        [InlineData("\\/L l\\/")]
        [InlineData("àaA")]
        [InlineData("Eéèe")]
        [InlineData("M@fd1523çççç\\\\èé/+ {}")]
        [InlineData("+-*/=")]
        public void ToObjet_set_private_set_string_property_check(string value)
        {
            var values = new Dictionary<string, StringValues>
            {
                { nameof(DummyObject.StringPrivateSetProperty), JsonSerializer.Serialize(value) }
            };
            var obj = (DummyObject)values.ToObject(typeof(DummyObject));
            obj.StringPrivateSetProperty.Should().Be(value);
        }

        [Theory]
        [InlineData("L l")]
        [InlineData("/L l/")]
        [InlineData("\\/L l\\/")]
        [InlineData("àaA")]
        [InlineData("Eéèe")]
        [InlineData("M@fd1523çççç\\\\èé/+ {}")]
        [InlineData("+-*/=")]
        public void ToObjet_set_readonly_string_property_check(string value)
        {
            var values = new Dictionary<string, StringValues>
            {
                { nameof(DummyObject.StringReadOnlyProperty), JsonSerializer.Serialize(value) }
            };
            var obj = (DummyObject)values.ToObject(typeof(DummyObject));
            obj.StringReadOnlyProperty.Should().Be(value);
        }

        [Theory]
        [InlineData("L l")]
        [InlineData("/L l/")]
        [InlineData("\\/L l\\/")]
        [InlineData("àaA")]
        [InlineData("Eéèe")]
        [InlineData("M@fd1523çççç\\\\èé/+ {}")]
        [InlineData("+-*/=")]
        public void ToObjet_set_string_property_check(string value)
        {
            var values = new Dictionary<string, StringValues>
            {
                { nameof(DummyObject.StringProperty), JsonSerializer.Serialize(value) }
            };
            var obj = (DummyObject)values.ToObject(typeof(DummyObject));
            obj.StringProperty.Should().Be(value);
        }

        [Fact]
        public void Type_GetConcreteClasses()
        {
            var result = typeof(ITestInterface).GetConcreteClasses(this.GetType().Assembly);
            result.Should().HaveCount(6);
            result.Should().Contain(new Type[] { typeof(TestConcrete1), typeof(TestConcrete2), typeof(TestConcrete3), typeof(TestConcrete4), typeof(TestConcrete5), typeof(TestConcrete6) });
            result = typeof(ITestInterface).GetConcreteClasses(new Assembly[] { this.GetType().Assembly });
            result.Should().HaveCount(6);
            result.Should().Contain(new Type[] { typeof(TestConcrete1), typeof(TestConcrete2), typeof(TestConcrete3), typeof(TestConcrete4), typeof(TestConcrete5), typeof(TestConcrete6) });
        }

        [Fact]
        public void Type_GetConcreteClasses_NoClasses()
        {
            var result = typeof(ITestInterfaceNoClass).GetConcreteClasses(this.GetType().Assembly);
            result.Should().HaveCount(0);
            result = typeof(ITestInterfaceNoClass).GetConcreteClasses(new Assembly[] { this.GetType().Assembly });
            result.Should().HaveCount(0);
        }

        [Fact]
        public void Type_GetConcreteClasses_NotInterface()
        {
            typeof(TestConcrete1)
                .Invoking(p => p.GetConcreteClasses(this.GetType().Assembly))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void Type_GetInterfaceGenericArguments_int()
        {
            var result = typeof(TestConcrete4).GetInterfaceGenericArguments(typeof(ITestInterface<>));
            result.Should().HaveCount(1);
            result.Should().Contain(new Type[] { typeof(int) });
            result = typeof(TestConcrete5).GetInterfaceGenericArguments(typeof(ITestInterface<>));
            result.Should().HaveCount(1);
            result.Should().Contain(new Type[] { typeof(string) });
        }

        [Fact]
        public void Type_GetInterfaceGenericArguments_NotAssignable()
        {
            typeof(TestConcrete1)
                .Invoking(p => p.GetInterfaceGenericArguments(typeof(ITestInterface<>)))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void Type_GetInterfaceGenericArguments_NotGeneric()
        {
            typeof(ITestInterface)
                .Invoking(p => p.GetInterfaceGenericArguments(typeof(ITestInterface)))
                .Should()
                .Throw<ArgumentException>();
        }
    }
}