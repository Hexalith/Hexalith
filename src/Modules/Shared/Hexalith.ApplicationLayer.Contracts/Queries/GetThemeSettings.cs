namespace Hexalith.ApplicationLayer.Queries
{
    using System.Runtime.Serialization;

    using Hexalith.Domain.Contracts.Projections;

    using ProtoBuf;

    [ProtoContract]
    [DataContract]
    [Query]
    public class GetThemeSettings
    {
    }
}