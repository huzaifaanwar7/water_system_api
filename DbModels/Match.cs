using System.ComponentModel.DataAnnotations;

namespace GBS.Api.DbModels
{
    public class Match
    {
        [Key]
        public int Id { get; set; }

        public int? TournamentId { get; set; }

        [Required, StringLength(200)]
        public string MatchName { get; set; } = string.Empty;

        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }

        [StringLength(150)] public string? Venue { get; set; }

        public DateTime ScheduledStart { get; set; }
        public DateTime? ActualStart { get; set; }
        public DateTime? ActualEnd { get; set; }

        // T10, T20, ODI, Custom
        [StringLength(20)] public string MatchFormat { get; set; } = "T20";
        public int OversPerInnings { get; set; } = 20;
        public int BallsPerOver { get; set; } = 6;

        // Penalty runs awarded to the opposition for late arrival / slow over rate / etc.
        public int HomePenaltyRuns { get; set; } = 0;
        public int AwayPenaltyRuns { get; set; } = 0;
        [StringLength(200)] public string? PenaltyReason { get; set; }

        public int? TossWinnerTeamId { get; set; }
        [StringLength(10)] public string? TossDecision { get; set; } // Bat / Bowl

        // Scheduled, Live, InningsBreak, Completed, Abandoned, Cancelled
        [StringLength(20)]
        public string MatchState { get; set; } = "Scheduled";

        public int? ResultWinnerTeamId { get; set; }
        [StringLength(100)] public string? ResultMargin { get; set; }
        public int? ManOfTheMatchPlayerId { get; set; }
        [StringLength(50)] public string? StageLabel { get; set; }

        public int? CreatedByUserId { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
