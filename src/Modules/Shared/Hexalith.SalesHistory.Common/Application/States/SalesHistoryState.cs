namespace Hexalith.SalesHistory.Common.Application.States
{
    using System;

    public class SalesHistoryState
    {
        public string CompanyId { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        public string Currency { get; set; } = string.Empty;

        public string CustomerId { get; set; } = string.Empty;

        public string CustomerName { get; set; } = string.Empty;

        public DateTimeOffset InvoiceDate { get; set; }

        public string InvoiceId { get; set; } = string.Empty;

        public string ItemId { get; set; } = string.Empty;

        public string? ItemName { get; set; }

        public string LineId { get; set; } = string.Empty;

        public decimal Quantity { get; set; }

        public string SalesId { get; set; } = string.Empty;

        public decimal TotalAmount { get; set; }
    }
}