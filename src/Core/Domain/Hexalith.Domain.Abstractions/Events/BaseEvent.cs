// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Domain.Abstractions.Events;

using Hexalith.Domain.Abstractions.Messages;

using System.Runtime.Serialization;

/// <summary>
/// Base class for business events.
/// </summary>
[DataContract]
public abstract class BaseEvent : Message, IEvent
{
}