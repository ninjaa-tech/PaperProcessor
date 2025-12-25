using System.ComponentModel.DataAnnotations;

namespace PaperProcessor.Models
{
    public class StageLog
    {
        public int StageLogId { get; set; }

        [Required]
        public int WorkOrderId { get; set; }
        public WorkOrder? WorkOrder { get; set; }

        [Required]
        public int ProductionStageId { get; set; }
        public ProductionStage? ProductionStage { get; set; }

        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }

        [Range(0, 100000000)]
        public int QtyOut { get; set; }

        [Range(0, 100000000)]
        public int ScrapQty { get; set; }

        [StringLength(250)]
        public string? Notes { get; set; }
    }
}
