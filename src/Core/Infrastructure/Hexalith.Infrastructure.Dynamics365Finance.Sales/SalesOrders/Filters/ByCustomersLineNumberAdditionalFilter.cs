namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesOrders.Filters;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

public record ByCustomersLineNumberAdditionalFilter(string DataAreaId, string SalesId, decimal LineNum)
    : PerCompanyFilter(DataAreaId)
{
}