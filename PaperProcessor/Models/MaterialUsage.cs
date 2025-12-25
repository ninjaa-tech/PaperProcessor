using System.ComponentModel.DataAnnotations;

namespace PaperProcessor.Models
{
    public class MaterialUsage
    {
        public int MaterialUsageId { get; set; }

        [Required]
        public int WorkOrderId { get; set; }
        public WorkOrder? WorkOrder { get; set; }

        [Required]
        public int MaterialId { get; set; }
        public Material? Material { get; set; }

        [Range(0, 100000000)]
        public decimal QuantityUsed { get; set; }

        // snapshot for costing accuracy
        [Range(0, 1000000)]
        public decimal CostPerUnitSnapshot { get; set; }

        public DateTime UsedAt { get; set; } = DateTime.UtcNow;
    }
}
