namespace Hexalith.Roles.Application.Domain.Commands
{
    using Hexalith.Roles.Application.Commands;
    using Hexalith.Roles.Domain.ValueTypes;

    public sealed class RenameRole
    {
        public RenameRole(RoleId unitId, string newName)
        {
            UnitId = unitId;
            NewName = newName;
        }

        public RoleId UnitId { get; }
        public string NewName { get; }
    }
}