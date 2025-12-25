using System.ComponentModel.DataAnnotations;

namespace PaperProcessor.Models
{
    public class ProductionStage
    {
        public int ProductionStageId { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public int SortOrder { get; set; }
    }
}
