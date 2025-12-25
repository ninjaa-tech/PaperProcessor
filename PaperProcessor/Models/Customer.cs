using System.ComponentModel.DataAnnotations;

namespace PaperProcessor.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required, StringLength(120)]
        public string Name { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(150)]
        public string? Email { get; set; }

        [StringLength(250)]
        public string? Address { get; set; }
    }
}
