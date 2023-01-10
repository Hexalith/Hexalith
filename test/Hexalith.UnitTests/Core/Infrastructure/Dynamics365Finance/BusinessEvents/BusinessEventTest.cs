// <copyright file="BusinessEventTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.Dynamics365Finance.BusinessEvents;

using FluentAssertions;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.BusinessEvents;

using System.Text.Json;

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
            "MajorVersion": 10,
            "MinorVersion": 11,
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

    private readonly string _typedBusinessEventJson1 =
        $$"""
        {
            "$type": "DummyBusinessEvent1",
            "BusinessEventId": "DummyBusinessEvent1",
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
            "ValueOne": "The one value"
        }
        """;

    private readonly string _typedBusinessEventJson2 =
        $$"""
        {
            "$type": "DummyBusinessEvent2",
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
    public void Dummy_event_one_add_and_deserialize_should_succeed()
    {
        using JsonDocument json = JsonDocument.Parse(_businessEventJson1);
        Dynamics365BusinessEventBase be = Dynamics365BusinessEventBase.AddTypeAndDeserialize(json.RootElement);
        _ = be.Should().NotBeNull();
        _ = be.Should().BeOfType<DummyBusinessEvent1>();
        _ = be.BusinessEventId.Should().Be("DummyBusinessEvent1");
        _ = ((DummyBusinessEvent1)be).ValueOne.Should().Be("The one value");
    }

    [Fact]
    public void Dummy_event_one_deserialization_should_succeed()
    {
        Dynamics365BusinessEventBase be = Dynamics365BusinessEventBase.Deserialize(_typedBusinessEventJson1);
        _ = be.Should().NotBeNull();
        _ = be.Should().BeOfType<DummyBusinessEvent1>();
        _ = be.BusinessEventId.Should().Be("DummyBusinessEvent1");
        _ = ((DummyBusinessEvent1)be).ValueOne.Should().Be("The one value");
    }

    [Fact]
    public void Dummy_event_two_add_and_deserialize_should_succeed()
    {
        using JsonDocument json = JsonDocument.Parse(_businessEventJson2);
        Dynamics365BusinessEventBase be = Dynamics365BusinessEventBase.AddTypeAndDeserialize(json.RootElement);
        _ = be.Should().NotBeNull();
        _ = be.Should().BeOfType<DummyBusinessEvent2>();
        _ = be.BusinessEventId.Should().Be("DummyBusinessEvent2");
        _ = ((DummyBusinessEvent2)be).ValueTwo.Should().Be(2);
    }

    [Fact]
    public void Dummy_event_two_deserialization_should_succeed()
    {
        Dynamics365BusinessEventBase be = Dynamics365BusinessEventBase.Deserialize(_typedBusinessEventJson2);
        _ = be.Should().NotBeNull();
        _ = be.Should().BeOfType<DummyBusinessEvent2>();
        _ = be.BusinessEventId.Should().Be("DummyBusinessEvent2");
        _ = ((DummyBusinessEvent2)be).ValueTwo.Should().Be(2);
    }
}
