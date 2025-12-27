using System;
using System.Collections.Generic;

namespace GBS.Api.DbModels;

public partial class ApprovalHistory
{
    public int Id { get; set; }

    public string? ObjectType { get; set; }

    public int? ObjectId { get; set; }

    public int? Stage { get; set; }

    public string? Remarks { get; set; }

    public bool IsDeleted { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? PendingWith { get; set; }

    public string? WaitingFor { get; set; }

    public string? LegacyUser { get; set; }

    public string? DoneByName { get; set; }

    public bool? IsBulkApprove { get; set; }
}
