namespace Hexalith.Emails.Application.Queries
{
    using Hexalith.Domain.Contracts.Projections;

    using ProtoBuf;

    [ProtoContract]
    [Query]
    public sealed class GetEmailDetails
    {
        [ProtoMember(1)]
        public string EmailId { get; set; } = string.Empty;
    }
}