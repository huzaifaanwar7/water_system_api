using System.ComponentModel.DataAnnotations;

namespace GBS.Api.DbModels
{
    // Generic approval feed for SuperAdmin: User (Captain signup), Team, Player
    public class ApprovalRequest
    {
        [Key]
        public int Id { get; set; }

        // User / Team / Player
        [Required, StringLength(30)]
        public string EntityType { get; set; } = string.Empty;

        public int EntityId { get; set; }

        public int RequestedByUserId { get; set; }

        // Pending / Approved / Rejected
        [Required, StringLength(20)]
        public string Status { get; set; } = "Pending";

        [StringLength(500)] public string? Notes { get; set; }
        [StringLength(500)] public string? RejectionReason { get; set; }

        public int? ReviewedByUserId { get; set; }
        public DateTime? ReviewedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
