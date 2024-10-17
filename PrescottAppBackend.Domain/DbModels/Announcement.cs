using System;
using System.Collections.Generic;

namespace PrescottAppBackend.Domain.DbModels;

public partial class Announcement
{
    public int Id { get; set; }

    public int BuildingId { get; set; }

    public string Title { get; set; } = null!;

    public string? Content { get; set; }

    public string? FileName { get; set; }

    public string? File { get; set; }

    public string? FileType { get; set; }

    public string? FilePath { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }
}
