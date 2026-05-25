namespace GBS.Api.DbModels
{
    public class FallOfWicket
    {
        public int Id { get; set; }
        public int InningsId { get; set; }
        public int BallId { get; set; }
        public int WicketNumber { get; set; }
        public int Runs { get; set; }
        public int LegalBallsAt { get; set; }
        public int DismissedPlayerId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
