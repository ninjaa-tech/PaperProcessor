using System.ComponentModel.DataAnnotations;

namespace PaperProcessor.Models
{
    public class Material
    {
        public int MaterialId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string Unit { get; set; } = "kg"; // kg, sheet, roll

        [Range(0, 1000000)]
        public decimal CostPerUnit { get; set; }
    }
}
