// <copyright file="TestMessageBase.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Serialization;

using Hexalith.Extensions.Serialization;

[JsonPolymorphicBaseType]
public record TestMessageBase(string Id, string Name)
{
}

[JsonPolymorphicBaseType]
public record TestMessageAbstractBase(string Id, string Name)
{
}

public record TestMessage1(string Id, string Name, string Value1) : TestMessageBase(Id, Name)
{
}

[JsonPolymorphicDerivedType("hello", 99)]
public record TestMessageWithCustomName(string Id, string Name, string MyValue) : TestMessageBase(Id, Name)
{
}

public record TestMessage2(string Id, string Name, string Value2) : TestMessageBase(Id, Name)
{
}

public record TestMessage3(string Id, string Name, string Value2, string Value3) : TestMessage2(Id, Name, Value2)
{
}

public record TestMessageWithAbstractBase1(string Id, string Name, string Value1) : TestMessageAbstractBase(Id, Name)
{
}

public record TestMessageWithAbstractBase2(string Id, string Name, string Value2) : TestMessageAbstractBase(Id, Name)
{
}

public record TestMessageWithAbstractBase3(string Id, string Name, string Value2, string Value3) : TestMessageWithAbstractBase2(Id, Name, Value2)
{
}