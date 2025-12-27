using System;
using System.Collections.Generic;

namespace GBS.Api.DbModels;

public partial class EmployeeLedger
{
    public int Id { get; set; }

    public int EmployeeIdFk { get; set; }

    public DateTime TransactionDate { get; set; }

    public int TransactionTypeIdFk { get; set; }

    public string? Description { get; set; }

    public decimal? DebitAmount { get; set; }

    public decimal? CreditAmount { get; set; }

    public string? SalaryMonth { get; set; }

    public decimal? RunningBalance { get; set; }

    public int StatusIdFk { get; set; }

    public bool IsRecovered { get; set; }

    public string? Remarks { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public bool IsActive { get; set; }

    public virtual Employee EmployeeIdFkNavigation { get; set; } = null!;

    public virtual Lookup StatusIdFkNavigation { get; set; } = null!;

    public virtual Lookup TransactionTypeIdFkNavigation { get; set; } = null!;
}
