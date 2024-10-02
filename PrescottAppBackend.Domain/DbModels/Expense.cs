using System;
using System.Collections.Generic;

namespace PrescottAppBackend.Domain.DbModels;

public partial class Expense
{
    public int Id { get; set; }

    public int BillId { get; set; }

    public string Account { get; set; } = null!;

    public decimal Amount { get; set; }

    public string? Memo { get; set; }

    public string? CustomerJob { get; set; }

    public bool? Billable { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public bool? IsDeleted { get; set; }
}
