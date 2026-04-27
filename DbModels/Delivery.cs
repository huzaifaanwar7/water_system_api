using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GBS.Api.DbModels
{
    public class Delivery
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        public int Bottles19L { get; set; }
        public int Bottles15L { get; set; }
        public int Bottles05L { get; set; }

        public int Empty19L { get; set; }
        public int Empty15L { get; set; }
        public int Empty05L { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [StringLength(50)]
        public string PaymentStatus { get; set; } = "pending"; // paid, pending, credit

        public string? Notes { get; set; }

        public int? InvoiceId { get; set; }

        [ForeignKey("InvoiceId")]
        public virtual Invoice? Invoice { get; set; }
    }
}
