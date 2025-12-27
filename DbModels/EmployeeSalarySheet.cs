using System;
using System.Collections.Generic;

namespace GBS.Api.DbModels;

public partial class EmployeeSalarySheet
{
    public int Id { get; set; }

    public int EmployeeIdFk { get; set; }

    public string SalaryMonth { get; set; } = null!;

    public decimal SalaryRate { get; set; }

    public int SickDays { get; set; }

    public int CasualDays { get; set; }

    public int WorkingDays { get; set; }

    public decimal DaysAmount { get; set; }

    public decimal OvertimeHours { get; set; }

    public decimal OvertimeAmount { get; set; }

    public decimal FuelAllowance { get; set; }

    public decimal Arrears { get; set; }

    public decimal MessDeduction { get; set; }

    public decimal? IncomeTaxDeduction { get; set; }

    public decimal AdvanceDeduction { get; set; }

    public decimal OtherDeductions { get; set; }

    public decimal TotalDeductions { get; set; }

    public decimal NetPayable { get; set; }

    public decimal PayableAmount { get; set; }

    public int StatusIdFk { get; set; }

    public DateTime? ProcessedDate { get; set; }

    public int? ProcessedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public bool IsActive { get; set; }

    public string? Remarks { get; set; }
}
