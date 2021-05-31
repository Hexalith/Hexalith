namespace Hexalith.Roles.Application.Commands
{
    using Hexalith.Roles.Domain.ValueTypes;

    public sealed class AddNewRole
    {
        public AddNewRole(RoleId unitId, string name, string? description)
        {
            Name = name;
            Description = description;
            UnitId = unitId;
        }

        public string? Description { get; }
        public string Name { get; }
        public string UnitId { get; }
    }
}