using System;
using System.Collections.Generic;

namespace PrescottAppBackend.Domain.DbModels;

public partial class AmenityImage
{
    public int Id { get; set; }

    public string FileName { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public string? FileType { get; set; }

    public int AmenityId { get; set; }
}
