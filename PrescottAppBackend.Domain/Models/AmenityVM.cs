using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrescottAppBackend.Domain.Models
{
    public class AmenityVM
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public string AmenityName { get; set; }
        public string? Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}