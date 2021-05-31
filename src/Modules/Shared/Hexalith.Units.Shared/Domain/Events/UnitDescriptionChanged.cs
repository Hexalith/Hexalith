namespace Hexalith.Units.Domain.Events
{
    using Hexalith.Units.Domain.ValueTypes;

    public sealed class UnitDescriptionChanged
    {
        public UnitDescriptionChanged(UnitId unitId, string? description)
        {
            Description = description;
            UnitId = unitId;
        }

        public string? Description { get; }
        public string UnitId { get; }
    }
}