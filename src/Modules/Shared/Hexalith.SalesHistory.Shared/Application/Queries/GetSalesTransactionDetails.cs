namespace Bistrotic.SalesHistory.Application.Queries
{
    using Bistrotic.SalesHistory.Application.ModelViews;
    using Bistrotic.SalesHistory.Domain.ValueTypes;

    public sealed class GetSalesTransactionDetails
    {
        public GetSalesTransactionDetails(SalesTransactionId salesTransactionId)
        {
            SalesTransactionId = salesTransactionId;
        }

        public SalesTransactionId SalesTransactionId { get; init; }
    }
}