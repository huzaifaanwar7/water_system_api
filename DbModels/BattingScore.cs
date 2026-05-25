using System.ComponentModel.DataAnnotations;

namespace GBS.Api.DbModels
{
    public class BattingScore
    {
        [Key] public int Id { get; set; }
        public int InningsId { get; set; }
        public int PlayerId { get; set; }
        public int BattingOrder { get; set; }
        public int Runs { get; set; } = 0;
        public int BallsFaced { get; set; } = 0;
        public int Fours { get; set; } = 0;
        public int Sixes { get; set; } = 0;
        public bool IsOut { get; set; } = false;
        [StringLength(200)] public string? DismissalDescription { get; set; }
        public int? DismissalBallId { get; set; }
    }
}
