namespace Hexalith.Units.Application.Queries
{
    using Hexalith.Units.Domain.ValueTypes;

    public sealed class GetUnitSummaryInformations
    {
        public GetUnitSummaryInformations(UnitId unitId)
        {
            UnitId = unitId;
        }

        public string UnitId { get; }
    }
}