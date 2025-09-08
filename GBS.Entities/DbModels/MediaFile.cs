using System;
using System.Collections.Generic;

namespace GBS.Entities.DbModels;

public partial class MediaFile
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? MimeType { get; set; }

    public long? Size { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Bucket { get; set; }

    public string? CreatedBy { get; set; }
}
