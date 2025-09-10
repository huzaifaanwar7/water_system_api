using System;
using System.Collections.Generic;

namespace GBS.Entities.DbModels;

public partial class Document
{
    public int Id { get; set; }

    public Guid MediaFileIdFk { get; set; }

    public string ObjectType { get; set; } = null!;

    public int ObjectId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public bool IsActive { get; set; }

    public string DocumentType { get; set; } = null!;

    public virtual MediaFile MediaFileIdFkNavigation { get; set; } = null!;
}
