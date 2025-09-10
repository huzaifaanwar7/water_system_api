using System;
using System.Collections.Generic;

namespace GBS.Entities.DbModels;

public partial class EmployeeSalaryRate
{
    public int Id { get; set; }

    public int EmployeeIdFk { get; set; }

    public DateOnly EffectiveDate { get; set; }

    public decimal SalaryRate { get; set; }

    public int ChangeTypeIdFk { get; set; }

    public decimal? PreviousRate { get; set; }

    public decimal? IncrementAmount { get; set; }

    public decimal? IncrementPercentage { get; set; }

    public int ApprovedBy { get; set; }

    public DateOnly ApprovalDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsActive { get; set; }

    public string? Remarks { get; set; }
}
