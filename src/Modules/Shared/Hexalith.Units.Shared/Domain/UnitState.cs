namespace Hexalith.Units.Domain
{
    using Hexalith.Domain;

    public sealed class UnitState : EntityState
    {
        public string? Description { get; set; }
        public string? Name { get; set; }
    }
}