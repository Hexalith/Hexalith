namespace Hexalith.ApplicationLayer.Queries
{
    using Hexalith.Domain.Contracts.Projections;

    using ProtoBuf;

    [ProtoContract]
    [Query]
    public sealed class GetApplicationName
    {
    }
}