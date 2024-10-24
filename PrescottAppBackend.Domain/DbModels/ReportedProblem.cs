using System;
using System.Collections.Generic;

namespace PrescottAppBackend.Domain.DbModels;

public partial class ReportedProblem
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Problem { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }
}
