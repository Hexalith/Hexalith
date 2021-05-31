namespace Hexalith.Roles.Domain.Events
{
    using Hexalith.Roles.Domain.ValueTypes;

    public sealed class RoleDescriptionChanged :
        RoleIdEvent
    {
        public RoleDescriptionChanged(RoleId unitId, string? description) : base(unitId)
        {
            Description = description;
        }

        public string? Description { get; }
    }
}