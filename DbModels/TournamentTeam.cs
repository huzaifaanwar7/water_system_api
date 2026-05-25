using System.ComponentModel.DataAnnotations;

namespace GBS.Api.DbModels
{
    public class TournamentTeam
    {
        [Key]
        public int Id { get; set; }

        public int TournamentId { get; set; }
        public int TeamId { get; set; }

        [StringLength(20)] public string? GroupName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
