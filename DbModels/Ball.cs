using System.ComponentModel.DataAnnotations;

namespace GBS.Api.DbModels
{
    public class Ball
    {
        [Key]
        public int Id { get; set; }

        public int InningsId { get; set; }
        public int MatchId { get; set; }

        public Guid BallGuid { get; set; } = Guid.NewGuid();
        public int OverNumber { get; set; }
        public int BallInOver { get; set; }
        public int BallSequence { get; set; }

        public int? StrikerPlayerId { get; set; }
        public int? NonStrikerPlayerId { get; set; }
        public int? BowlerPlayerId { get; set; }

        public int RunsBatter { get; set; } = 0;
        public int RunsExtras { get; set; } = 0;

        [StringLength(20)] public string? ExtrasType { get; set; } // Wide/NoBall/Bye/LegBye/Penalty
        public bool IsLegalDelivery { get; set; } = true;
        public bool IsFreeHit { get; set; } = false;

        public bool IsWicket { get; set; } = false;
        [StringLength(20)] public string? WicketType { get; set; }
        public int? DismissedPlayerId { get; set; }
        public int? FielderPlayerId { get; set; }

        [StringLength(500)] public string? Commentary { get; set; }

        public int? ScoredByUserId { get; set; }
        public bool IsUndone { get; set; } = false;

        public DateTime BowledAt { get; set; } = DateTime.UtcNow;
    }
}
