// <copyright file="BusinessEventTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.Dynamics365Finance.BusinessEvents;

using System.Text.Json;

using FluentAssertions;

using FluentValidation;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.BusinessEvents;

public class BusinessEventTest
{
    private readonly string _businessEventJson1 =
        $$"""
        {
            "BusinessEventId": "DummyBusinessEvent1",
            "BusinessEventLegalEntity": "SO01",
            "ContextRecordSubject": "",
            "ControlNumber": 5637611083,
            "DeliveryDate": "/Date(1672358400000)/",
            "EventId": "3C47A4CD-6EDC-4319-B893-3F862EED65FD",
            "EventTime": "/Date(1672408275000)/",
            "EventTimeIso8601": "2022-12-30T13:51:15.2942742Z",
            "InitiatingUserAADObjectId": "{410A2690-5EC2-42EE-9F2A-E75B4E324930}",
            "MajorVersion": 6,
            "MinorVersion": 7,
            "ParentContextRecordSubjects": [],
            "ValueOne": "The one value"
        }
        """;

    private readonly string _businessEventJson2 =
        $$"""
        {
            "BusinessEventId": "DummyBusinessEvent2",
            "BusinessEventLegalEntity": "SO01",
            "ContextRecordSubject": "",
            "ControlNumber": 5637611083,
            "DeliveryDate": "/Date(1672358400000)/",
            "EventId": "3C47A4CD-6EDC-4319-B893-3F862EED65FD",
            "EventTime": "/Date(1672408275000)/",
            "EventTimeIso8601": "2022-12-30T13:51:15.2942742Z",
            "InitiatingUserAADObjectId": "{410A2690-5EC2-42EE-9F2A-E75B4E324930}",
            "MajorVersion": 10,
            "MinorVersion": 11,
            "ParentContextRecordSubjects": [],
            "ValueTwo": 2
        }
        """;

    // test error message serialization
    [Fact]
    public void Base_event_deserialize_should_succeed()
    {
        DateTimeOffset date = new(2022, 12, 30, 13, 51, 15, 294, TimeSpan.Zero);
        Dynamics365BusinessEventBase be = JsonSerializer.Deserialize<Dynamics365BusinessEventBase>(_businessEventJson1);
        _ = be.Should().NotBeNull();
        _ = be.Should().BeOfType<DummyBusinessEvent1>();
        _ = be.BusinessEventId.Should().Be("DummyBusinessEvent1");
        _ = be.BusinessEventLegalEntity.Should().Be("SO01");
        _ = be.ControlNumber.Should().Be(5637611083);
        _ = be.EventId.Should().Be("3C47A4CD-6EDC-4319-B893-3F862EED65FD");
        _ = be.EventTime
                .Should()
                .Be(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddMilliseconds(1672408275000));
        _ = be.InitiatingUserAzureActiveDirectoryObjectId.Should().Be("{410A2690-5EC2-42EE-9F2A-E75B4E324930}");
        _ = be.MajorVersion.Should().Be(6);
        _ = be.MinorVersion.Should().Be(7);
        _ = be.EventTimeIso8601.Should().BeCloseTo(date, TimeSpan.FromMilliseconds(1));
        _ = be.Context.Should().NotBeNull();
        _ = be.Context.ReceivedDate.Should().NotBeNull();
        _ = be.Context.ReceivedDate.Should().Be(be.EventTime);
    }

    [Fact]
    public void Data_contract_serialize_and_deserialize_should_return_same_object()
    {
        Dynamics365BusinessEventBase original = GetEvent();

        _ = original.Should().BeDataContractSerializable();
    }

    [Fact]
    public void Dummy_event_one_deserialization_should_succeed()
    {
        DummyBusinessEvent1 be = JsonSerializer.Deserialize<DummyBusinessEvent1>(_businessEventJson1);
        _ = be.Should().NotBeNull();
        _ = be.Should().BeOfType<DummyBusinessEvent1>();
        _ = be.BusinessEventId.Should().Be("DummyBusinessEvent1");
        _ = be.ValueOne.Should().Be("The one value");
    }

    [Fact]
    public void Dummy_event_one_polymorphic_deserialization_should_succeed()
    {
        Dynamics365BusinessEventBase be = JsonSerializer.Deserialize<Dynamics365BusinessEventBase>(_businessEventJson1);
        _ = be.Should().NotBeNull();
        _ = be.Should().BeOfType<DummyBusinessEvent1>();
        _ = be.BusinessEventId.Should().Be("DummyBusinessEvent1");
        _ = ((DummyBusinessEvent1)be).ValueOne.Should().Be("The one value");
    }

    [Fact]
    public void Dummy_event_two_deserialization_should_succeed()
    {
        DummyBusinessEvent2 be = JsonSerializer.Deserialize<DummyBusinessEvent2>(_businessEventJson2);
        _ = be.Should().NotBeNull();
        _ = be.Should().BeOfType<DummyBusinessEvent2>();
        _ = be.BusinessEventId.Should().Be("DummyBusinessEvent2");
        _ = be.ValueTwo.Should().Be(2);
    }

    [Fact]
    public void Dummy_event_two_polymorphic_deserialization_should_succeed()
    {
        Dynamics365BusinessEventBase be = JsonSerializer.Deserialize<Dynamics365BusinessEventBase>(_businessEventJson2);
        _ = be.Should().NotBeNull();
        _ = be.Should().BeOfType<DummyBusinessEvent2>();
        _ = be.BusinessEventId.Should().Be("DummyBusinessEvent2");
        _ = ((DummyBusinessEvent2)be).ValueTwo.Should().Be(2);
    }

    [Fact]
    public void Event_validation_should_succeed()
    {
        Dynamics365BusinessEventBase be = GetEvent();
        Dynamics365BusinessEventValidator validator = new();
        validator.ValidateAndThrow(be);
    }

    [Fact]
    public void Event_with_undefined_date_time_validation_should_fail()
    {
        // A business event with an undefined event time should throw an exception
        DummyBusinessEvent2 be = new()
        {
            EventTime = null,
            EventTimeIso8601 = null,
        };
        Dynamics365BusinessEventValidator validator = new();
        FluentValidation.Results.ValidationResult result = validator.Validate(be);
        _ = result.IsValid.Should().BeFalse();
        _ = result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(Dynamics365BusinessEventBase.EventTime));
    }

    [Fact]
    public void Serialize_and_deserialize_should_return_same_object()
    {
        Dynamics365BusinessEventBase original = GetEvent();
        string json = JsonSerializer.Serialize(original);
        Dynamics365BusinessEventBase result = JsonSerializer.Deserialize<Dynamics365BusinessEventBase>(json);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(original);
    }

    private static Dynamics365BusinessEventBase GetEvent()
    {
        DateTime date = new(2023, 02, 01, 16, 31, 40, DateTimeKind.Utc);
        return new DummyBusinessEvent2()
        {
            EventTimeIso8601 = date,
            BusinessEventId = "DummyBusinessEvent2",
            BusinessEventLegalEntity = "CPY",
            ControlNumber = 1235632,
            EventId = "Ev123",
            EventTime = date.AddSeconds(-1),
            InitiatingUserAzureActiveDirectoryObjectId = "me",
            MajorVersion = 10,
            MinorVersion = 11,
        };
    }
}