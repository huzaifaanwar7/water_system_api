using System;
using System.ComponentModel.DataAnnotations;
using PrescottAppBackend.Domain.DbModels;

namespace PrescottAppBackend.Domain
{

    public class AnnouncementVM
    {
        public int Id { get; set; }

        public int BuildingId { get; set; }

        public string Title { get; set; } = null!;

        public string? Content { get; set; }

        public string? FileName { get; set; }

        public string? FilePath { get; set; }

        public string? File { get; set; }

        public string? FileType { get; set; }

        public byte[] Document { get; set; } = [];

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public string? BuildingName { get; set; }
        
        public string? CreatedByStr { get; set; }

        public User UserVM { get; set; }
    }
}