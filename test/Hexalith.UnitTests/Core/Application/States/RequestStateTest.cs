// <copyright file="RequestStateTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.States;

using System;
using System.Text.Json;

using FluentAssertions;

using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Extensions.Helpers;
using Hexalith.Extensions.Serialization;
using Hexalith.UnitTests.Core.Application.Requests;

using Xunit;

public class RequestStateTest
{
    [Fact]
    public void StateSerializationAndDeserializationShouldReturnSameObject()
    {
        DummyRequest1 request = new();
        Hexalith.Application.Metadatas.Metadata meta = request.CreateMetadata();
        RequestState state = new(DateTimeOffset.UtcNow, request, meta);
        string json = JsonSerializer.Serialize(state);
        _ = json.Should().NotBeNullOrEmpty();
        RequestState result = JsonSerializer.Deserialize<RequestState>(json);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(state);
    }

    [Fact]
    public void StateSerializationShouldSucceed()
    {
        DummyRequest1 request = new();
        Hexalith.Application.Metadatas.Metadata meta = request.CreateMetadata();
        RequestState state = new(DateTimeOffset.UtcNow, request, meta);
        string json = JsonSerializer.Serialize(state);
        _ = json.Should().NotBeNullOrEmpty();
        _ = json.Should().Contain($"\"{IPolymorphicSerializable.TypeNamePropertyName}\":\"{nameof(DummyRequest1)}\"");
        _ = json.Should().Contain($"\"{IPolymorphicSerializable.TypeNamePropertyName}\":\"{nameof(Metadata)}\"");
        _ = json.Should().Contain($"\"{nameof(meta.Message.Id)}\":\"{meta.Message.Id}\"");
        _ = json.Should().Contain($"\"{nameof(request.Value1)}\":{request.Value1.ToInvariantString()}");
        _ = json.Should().Contain($"\"{nameof(request.BaseValue)}\":\"{request.BaseValue}\"");
    }
}