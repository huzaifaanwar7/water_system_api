using System.ComponentModel.DataAnnotations;

namespace GBS.Api.DbModels
{
    public class TeamPlayer
    {
        [Key]
        public int Id { get; set; }

        public int TeamId { get; set; }
        public int PlayerId { get; set; }
        public int? JerseyNumber { get; set; }

        [StringLength(20)]
        public string? Season { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
