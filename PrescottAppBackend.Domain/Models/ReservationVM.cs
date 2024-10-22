using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrescottAppBackend.Domain.DbModels;

namespace PrescottAppBackend.Domain
{
    public class ReservationVM
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public int AmenityId { get; set; }
        public string? Reason { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Amenity? Amenities { get; set; }
        public Building? Building { get; set; }
        public User? UserVM { get; set; }
        public List<AmenityImageVM> AmenityImages { get; set; } = new();
    }
}