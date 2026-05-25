using System.ComponentModel.DataAnnotations;

namespace GBS.Api.DbModels
{
    public class Tournament
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(50)] public string? Edition { get; set; }
        [StringLength(50)] public string? Category { get; set; }

        // RoundRobin, Knockout, Hybrid
        [StringLength(20)] public string? Format { get; set; }

        // T10, T20, ODI, Custom
        [StringLength(20)] public string? MatchFormat { get; set; }

        public int? OversPerInnings { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Registration, GroupStage, QuarterFinals, SemiFinals, Final, Completed
        [StringLength(30)]
        public string Stage { get; set; } = "Registration";

        public string? LogoUrl { get; set; }

        public int? CreatedByUserId { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
