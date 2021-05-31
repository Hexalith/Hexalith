namespace Hexalith.MudBlazorTheme.Queries
{
    using Hexalith.Domain.Contracts.Projections;

    using ProtoBuf;

    [ProtoContract]
    [Query]
    public sealed class GetMudBlazorThemeSetup
    {
        [ProtoMember(1)]
        public string UserName { get; set; } = string.Empty;

    }
}