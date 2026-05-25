using System.ComponentModel.DataAnnotations;

namespace GBS.Api.DbModels
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Username { get; set; } = string.Empty;

        [StringLength(150)]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        [Required, StringLength(255)]
        public string Password { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        public string? AvatarUrl { get; set; }

        // SuperAdmin, Captain, Scorer, Player, Fan
        [Required, StringLength(30)]
        public string Role { get; set; } = "Fan";

        // Captain signups need SuperAdmin approval. Pending/Approved/Rejected
        [StringLength(20)]
        public string ApprovalStatus { get; set; } = "Approved";

        public int? ApprovedByUserId { get; set; }
        public DateTime? ApprovedAt { get; set; }

        public int? LinkedPlayerId { get; set; }
        public int? TeamId { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
