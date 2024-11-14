using System;
using System.ComponentModel.DataAnnotations;

namespace GlobularsAdmin.Domain
{
    public class BuildingVM
    {
        [Key]
        public int Id { get; set; }

        public string BuildingName { get; set; } = null!;

        public string? BuildingDescription { get; set; }

        public string? Address { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }
    }
}