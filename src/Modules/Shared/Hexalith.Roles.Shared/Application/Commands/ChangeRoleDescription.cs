namespace Hexalith.Roles.Application.Domain.Commands
{
    using Hexalith.Roles.Application.Commands;
    using Hexalith.Roles.Domain.ValueTypes;

    public sealed class ChangeRoleDescription
    {
        public ChangeRoleDescription(RoleId unitId, string? description)
        {
            Description = description;
            UnitId = unitId;
        }

        public string? Description { get; }
        public string UnitId { get; }
    }
}