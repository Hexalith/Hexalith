namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesOrders.Filters;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

public record ByCustomersLineNumberFilter(string DataAreaId, string SalesOrderNumber, int CustomersLineNumber)
    : PerCompanyFilter(DataAreaId)
{
}