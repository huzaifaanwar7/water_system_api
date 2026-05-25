using System.ComponentModel.DataAnnotations;

namespace GBS.Api.DbModels
{
    public class Notification
    {
        [Key] public int Id { get; set; }
        // null = broadcast to all
        public int? UserId { get; set; }
        [Required, StringLength(100)] public string Type { get; set; } = "Info"; // Wicket / Fifty / Hundred / MatchStart / MatchEnd / TournamentStage / Announcement
        [Required, StringLength(150)] public string Title { get; set; } = "";
        [StringLength(500)] public string? Body { get; set; }
        public int? MatchId { get; set; }
        public int? TournamentId { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
