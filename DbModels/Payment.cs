using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GBS.Api.DbModels
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [StringLength(50)]
        public string Method { get; set; } = "cash"; // cash, bank, jazzcash

        [StringLength(100)]
        public string? TxnId { get; set; }

        public string? Notes { get; set; }

        public int? InvoiceId { get; set; }

        [ForeignKey("InvoiceId")]
        public virtual Invoice? Invoice { get; set; }
    }
}
