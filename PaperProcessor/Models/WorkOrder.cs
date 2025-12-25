using System.ComponentModel.DataAnnotations;

namespace PaperProcessor.Models
{
    public enum WorkOrderStatus
    {
        Draft = 0,
        Approved = 1,
        InProduction = 2,
        Completed = 3,
        Invoiced = 4,
        Cancelled = 5
    }

    public class WorkOrder
    {
        public int WorkOrderId { get; set; }

        [Required, StringLength(30)]
        public string WorkOrderNo { get; set; } = string.Empty; // WO-2026-0001

        [Required]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        [Required]
        public int ProductCartonId { get; set; }
        public ProductCarton? ProductCarton { get; set; }

        [Range(1, 100000000)]
        public int QuantityOrdered { get; set; }

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; } = DateTime.Today.AddDays(7);

        // Selling price (no GST)
        [Range(0, 100000000)]
        public decimal UnitSellPrice { get; set; }

        public WorkOrderStatus Status { get; set; } = WorkOrderStatus.Draft;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
