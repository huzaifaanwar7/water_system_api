using System.ComponentModel.DataAnnotations;

namespace GBS.Api.DbModels
{
    public class Commentary
    {
        public int Id { get; set; }
        public int BallId { get; set; }
        public int MatchId { get; set; }
        [Required, StringLength(1000)] public string Text { get; set; } = "";
        public bool IsMilestone { get; set; } = false;
        [StringLength(30)] public string? MilestoneType { get; set; }
        public bool IsOverridden { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
