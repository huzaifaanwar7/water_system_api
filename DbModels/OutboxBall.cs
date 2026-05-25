namespace GBS.Api.DbModels
{
    // Reserved for server-side audit of offline-pushed balls. Currently informational only.
    public class OutboxBall
    {
        public int Id { get; set; }
        public int? BallId { get; set; }
        public Guid BallGuid { get; set; }
        public int MatchId { get; set; }
        public int InningsId { get; set; }
        public string? ClientDeviceId { get; set; }
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    }
}
