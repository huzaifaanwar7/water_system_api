using System;
using System.Collections.Generic;

namespace GBS.Api.DbModels;

public partial class MediaFile
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? MimeType { get; set; }

    public long? Size { get; set; }

    public string? Bucket { get; set; }

    public string? Application { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
}
