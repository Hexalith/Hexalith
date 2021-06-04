namespace Hexalith.ApplicationLayer.Queries
{
    using System.Runtime.Serialization;

    using Hexalith.Domain.Contracts.Projections;

    using ProtoBuf;

    [ProtoContract]
    [DataContract]
    [Query]
    public class GetUserInterfaceTheme
    {
        [ProtoMember(1)]
        [DataMember(Order = 0)]
        public string UserName { get; set; } = default!;
    }
}