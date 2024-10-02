using System;
using System.ComponentModel.DataAnnotations;

namespace PrescottAppBackend.Domain
{
    public class DropdownListParentVM
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public List<DropdownListChildVM> dropdownListChildren { get; set; }

    }
}


