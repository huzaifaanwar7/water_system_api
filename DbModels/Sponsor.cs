using System.ComponentModel.DataAnnotations;

namespace GBS.Api.DbModels
{
    public class Sponsor
    {
        [Key] public int Id { get; set; }
        [Required, StringLength(150)] public string Name { get; set; } = "";
        [StringLength(200)] public string? Tagline { get; set; }
        [StringLength(500)] public string? LogoUrl { get; set; }
        [StringLength(500)] public string? WebsiteUrl { get; set; }
        [StringLength(30)]  public string? ContactPhone { get; set; }
        // Comma-separated slots: Splash, Dashboard, Scorecard, Commentary, OverCard, Kit, MatchPresentedBy
        [StringLength(300)] public string? Slots { get; set; }
        public int? TournamentId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
