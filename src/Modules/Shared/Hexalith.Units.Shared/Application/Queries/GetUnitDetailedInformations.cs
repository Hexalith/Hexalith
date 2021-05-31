namespace Hexalith.Units.Application.Queries
{
    using Hexalith.Units.Domain.ValueTypes;

    public sealed class GetUnitDetailedInformations
    {
        public GetUnitDetailedInformations(UnitId unitId)
        {
            UnitId = unitId;
        }

        public string UnitId { get; }
    }
}