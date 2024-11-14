using System;
using System.Collections.Generic;

namespace GlobularsAdmin.Domain.DbModels;

public partial class AnnouncementImage
{
    public int Id { get; set; }

    public string FileName { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public string? FileType { get; set; }

    public int AnnouncementId { get; set; }
}
