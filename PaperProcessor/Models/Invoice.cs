using System.ComponentModel.DataAnnotations;

namespace PaperProcessor.Models
{
    public enum InvoiceStatus
    {
        Draft = 0,
        Sent = 1,
        Paid = 2,
        Cancelled = 3
    }

    public class Invoice
    {
        public int InvoiceId { get; set; }

        [Required]
        public int WorkOrderId { get; set; }
        public WorkOrder? WorkOrder { get; set; }

        [Required, StringLength(30)]
        public string InvoiceNo { get; set; } = string.Empty; // INV-2025-0001

        public DateTime IssueDate { get; set; } = DateTime.Today;
        public DateTime DueDate { get; set; } = DateTime.Today.AddDays(14);

        public decimal Subtotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Total { get; set; }

        public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<InvoiceLine> Lines { get; set; } = new();
    }
}
