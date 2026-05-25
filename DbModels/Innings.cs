using System.ComponentModel.DataAnnotations;

namespace GBS.Api.DbModels
{
    public class Innings
    {
        [Key]
        public int Id { get; set; }

        public int MatchId { get; set; }
        public int InningsNumber { get; set; } // 1 or 2

        public int BattingTeamId { get; set; }
        public int BowlingTeamId { get; set; }

        public int TotalRuns { get; set; } = 0;
        public int Wickets { get; set; } = 0;
        public int LegalBallsBowled { get; set; } = 0;

        public int ExtrasWides { get; set; } = 0;
        public int ExtrasNoBalls { get; set; } = 0;
        public int ExtrasByes { get; set; } = 0;
        public int ExtrasLegByes { get; set; } = 0;
        public int ExtrasPenalty { get; set; } = 0;

        public int? Target { get; set; }
        public bool IsClosed { get; set; } = false;
        public DateTime? ClosedAt { get; set; }
        [StringLength(20)] public string? ClosedReason { get; set; } // AllOut / OversComplete / TargetReached / Abandoned

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
