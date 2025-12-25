using System.ComponentModel.DataAnnotations;

namespace PaperProcessor.Models
{
    public class InvoiceLine
    {
        public int InvoiceLineId { get; set; }

        [Required]
        public int InvoiceId { get; set; }
        public Invoice? Invoice { get; set; }

        [Required, StringLength(200)]
        public string Description { get; set; } = string.Empty;

        [Range(0, 100000000)]
        public decimal Qty { get; set; }

        [Range(0, 100000000)]
        public decimal UnitPrice { get; set; }

        public decimal LineTotal { get; set; }
    }
}
