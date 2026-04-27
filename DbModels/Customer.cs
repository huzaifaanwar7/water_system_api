using System.ComponentModel.DataAnnotations;

namespace GBS.Api.DbModels
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(500)]
        public string Address { get; set; } = string.Empty;

        [StringLength(100)]
        public string Area { get; set; } = string.Empty;

        [StringLength(50)]
        public string Type { get; set; } = "home"; // home, office, shop, pharmacy

        public decimal Balance { get; set; } = 0; // negative = udhaar, positive = advance

        public int BottlesOut { get; set; } = 0;

        public decimal OpeningBalance { get; set; } = 0;

        public string? Notes { get; set; }

        // Navigation properties
        public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
