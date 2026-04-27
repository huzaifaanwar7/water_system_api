using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GBS.Api.DbModels
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        [Required]
        public DateTime InvoiceDate { get; set; } = DateTime.Now;

        public DateTime? DueDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "pending"; // paid, pending

        public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
    }
}
