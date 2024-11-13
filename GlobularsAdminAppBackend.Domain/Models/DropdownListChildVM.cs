using System;
using System.ComponentModel.DataAnnotations;

namespace GlobularsAdminAppBackend.Domain
{
    public class DropdownListChildVM
    {
        [Key]
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

    }
}


