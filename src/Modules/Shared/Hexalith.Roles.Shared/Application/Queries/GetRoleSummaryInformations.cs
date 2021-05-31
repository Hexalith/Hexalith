namespace Hexalith.Roles.Application.Queries
{
    using Hexalith.Roles.Application.ModelViews;
    using Hexalith.Roles.Domain.ValueTypes;

    public sealed class GetRoleSummaryInformations : RoleIdQuery<RoleSummaryInformations>
    {
        public GetRoleSummaryInformations(RoleId unitId) : base(unitId)
        {
        }
    }
}