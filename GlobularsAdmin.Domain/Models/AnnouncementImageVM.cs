using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobularsAdmin.Domain
{
    public class AnnouncementImageVM
    {
        public int Id { get; set; }

        public string FileName { get; set; } = null!;

        public string? FilePath { get; set; }

        public string? FileType { get; set; }

        public int AnnouncementId { get; set; }

        public string File { get; set; } = null!;
    }
}