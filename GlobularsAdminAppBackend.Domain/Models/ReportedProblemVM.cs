using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobularsAdminAppBackend.Domain.DbModels;

namespace GlobularsAdminAppBackend.Domain
{
    public class ReportedProblemVM
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Problem { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public string CreatedByStr { get; set; } = "";

        public bool IsDeleted { get; set; }
        public User? UserVM { get; set; }
        public List<ReportedProblemImageVM> ReportedProblemImages { get; set; } = new();
    }
}