using System;
using System.ComponentModel.DataAnnotations;

namespace GlobularsAdminAppBackend.Domain
{
    public class RoleVM
    {
        [Key]
        public string? Id { get; set; }
        public string? RoleName { get; set; }
        public string? RoleDescription { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}