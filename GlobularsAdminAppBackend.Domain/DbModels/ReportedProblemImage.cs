using System;
using System.Collections.Generic;

namespace GlobularsAdminAppBackend.Domain.DbModels;

public partial class ReportedProblemImage
{
    public int Id { get; set; }

    public string FileName { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public string FileType { get; set; } = null!;

    public int ReportedProblemId { get; set; }
}
