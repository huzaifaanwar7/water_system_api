using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobularsAdmin.Domain
{
    public class AmenityImageVM
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string? FilePath { get; set; }

        public string? FileType { get; set; }

        public int AmenityId { get; set; }

        public string? File { get; set; }
    }
}