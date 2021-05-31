namespace Hexalith.Units.Application.Domain.Commands
{
    using Hexalith.Units.Domain.ValueTypes;

    public sealed class RenameUnit
    {
        public RenameUnit(UnitId unitId, string newName)
        {
            NewName = newName;
            UnitId = unitId;
        }

        public string NewName { get; }
        public string UnitId { get; }
    }
}