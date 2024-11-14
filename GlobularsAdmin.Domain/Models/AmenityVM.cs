using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobularsAdmin.Domain.DbModels;

namespace GlobularsAdmin.Domain
{
    public class AmenityVM
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public string AmenityName { get; set; }
        public string? Description { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? BuildingName { get; set; }
        public string? CreatedByStr { get; set; }
        public User? UserVM { get; set; }

        public List<AmenityImageVM> AmenityImages { get; set; } = new();
    }
}