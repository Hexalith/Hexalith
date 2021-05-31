namespace Hexalith.Roles.Domain
{
    using Hexalith.Domain;

    public sealed class RoleState : EntityState
    {
        public string? Description { get; set; }
        public string? Name { get; set; }
    }
}