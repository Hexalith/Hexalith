namespace Hexalith.Units.Application.Commands
{
    using Hexalith.Units.Domain.ValueTypes;

    public sealed class AddNewUnit
    {
        public AddNewUnit(UnitId unitId, string name, string? description)
        {
            UnitId = unitId;
            Name = name;
            Description = description;
        }

        public string? Description { get; }
        public string Name { get; }
        public string UnitId { get; }
    }
}