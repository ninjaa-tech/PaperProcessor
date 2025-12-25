using System.ComponentModel.DataAnnotations;

namespace PaperProcessor.Models
{
    public class ProductCarton
    {
        public int ProductCartonId { get; set; }

        [Required, StringLength(120)]
        public string Name { get; set; } = string.Empty;

        // Dimensions in mm (simple + realistic)
        [Range(1, 5000)]
        public int LengthMm { get; set; }

        [Range(1, 5000)]
        public int WidthMm { get; set; }

        [Range(1, 5000)]
        public int HeightMm { get; set; }

        [StringLength(80)]
        public string? Style { get; set; }  // e.g. Regular Slotted Container (RSC)

        [StringLength(250)]
        public string? Notes { get; set; }
    }
}
