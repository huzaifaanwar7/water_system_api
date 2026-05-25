namespace GBS.Api.DbModels
{
    public class BowlingFigure
    {
        public int Id { get; set; }
        public int InningsId { get; set; }
        public int PlayerId { get; set; }
        public int LegalBalls { get; set; } = 0;      // store legal balls; overs = balls/6
        public int Maidens { get; set; } = 0;
        public int RunsConceded { get; set; } = 0;
        public int Wickets { get; set; } = 0;
        public int Dots { get; set; } = 0;
        public int Fours { get; set; } = 0;
        public int Sixes { get; set; } = 0;
        public int Wides { get; set; } = 0;
        public int NoBalls { get; set; } = 0;
    }
}
