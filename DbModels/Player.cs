using System.ComponentModel.DataAnnotations;

namespace GBS.Api.DbModels
{
    public class Player
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        // Batter, Bowler, AllRounder, WicketKeeper
        [StringLength(30)]
        public string? Role { get; set; }

        // Right, Left
        [StringLength(10)]
        public string? BattingHandedness { get; set; }

        [StringLength(50)]
        public string? BowlingStyle { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        public string? PhotoUrl { get; set; }

        public int? TeamId { get; set; }
        public int? JerseyNumber { get; set; }

        // Pending / Approved / Rejected — captain adds, SuperAdmin approves
        [StringLength(20)]
        public string ApprovalStatus { get; set; } = "Pending";

        public int? CreatedByUserId { get; set; }
        public int? ApprovedByUserId { get; set; }
        public DateTime? ApprovedAt { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
