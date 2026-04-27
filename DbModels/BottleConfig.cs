using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GBS.Api.DbModels
{
    public class BottleConfig
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty; // e.g. "19L", "1.5L", "0.5L"

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int CurrentStock { get; set; } = 0;

        public string? Description { get; set; }
    }
}
