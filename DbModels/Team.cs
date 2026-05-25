using System.ComponentModel.DataAnnotations;

namespace GBS.Api.DbModels
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(120)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(4)]
        public string ShortCode { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Category { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? HomeVenue { get; set; }

        public short? FoundedYear { get; set; }

        [StringLength(7)] public string? PrimaryColorHex { get; set; }
        [StringLength(7)] public string? SecondaryColorHex { get; set; }

        public string? LogoUrl { get; set; }

        public int? CaptainUserId { get; set; }
        public int? CaptainPlayerId { get; set; }

        // Pending / Approved / Rejected — team itself requires SuperAdmin approval
        [StringLength(20)]
        public string ApprovalStatus { get; set; } = "Pending";

        public int? ApprovedByUserId { get; set; }
        public DateTime? ApprovedAt { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
