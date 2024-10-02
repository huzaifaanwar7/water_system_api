using System;
using System.Collections.Generic;

namespace PrescottAppBackend.Domain.DbModels;

public partial class IncomeTracker
{
    public int Id { get; set; }

    public int VendorId { get; set; }

    public DateTime BillDate { get; set; }

    public string? RefNo { get; set; }

    public decimal AmountDue { get; set; }

    public string? Terms { get; set; }

    public DateTime BillDue { get; set; }

    public string? Address { get; set; }

    public string? Memo { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public bool? IsDeleted { get; set; }
}
