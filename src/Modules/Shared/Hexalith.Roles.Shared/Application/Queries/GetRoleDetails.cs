namespace Hexalith.Roles.Application.Queries
{
    using Hexalith.Roles.Application.ModelViews;
    using Hexalith.Roles.Domain.ValueTypes;

    public sealed class GetRoleDetails
        : RoleIdQuery<RoleDetailedInformations>
    {
        public GetRoleDetails(RoleId unitId) : base(unitId)
        {
        }
    }
}