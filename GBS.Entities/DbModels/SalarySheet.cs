using System;
using System.Collections.Generic;

namespace GBS.Entities.DbModels;

public partial class SalarySheet
{
    public int Id { get; set; }

    public int EmployeeIdFk { get; set; }

    public string SalaryMonth { get; set; } = null!;

    public string EmployeeNumber { get; set; } = null!;

    public string EmployeeName { get; set; } = null!;

    public decimal SalaryRate { get; set; }

    public DateOnly DateOfJoining { get; set; }

    public int ActualDays { get; set; }

    public int SickDays { get; set; }

    public int CasualDays { get; set; }

    public int WorkingDays { get; set; }

    public decimal DaysAmount { get; set; }

    public decimal OvertimeHours { get; set; }

    public decimal OvertimeAmount { get; set; }

    public int LateDays { get; set; }

    public decimal LateAmount { get; set; }

    public decimal OvertimeExpense { get; set; }

    public decimal FuelAllowance { get; set; }

    public decimal Arrears { get; set; }

    public decimal MessTotal { get; set; }

    public decimal AdvanceDeduction { get; set; }

    public decimal OtherDeductions { get; set; }

    public decimal TotalDeductions { get; set; }

    public decimal GrossTotal { get; set; }

    public decimal NetPayable { get; set; }

    public decimal PayableAmount { get; set; }

    public string? Signature { get; set; }

    public int StatusIdFk { get; set; }

    public DateTime? ProcessedDate { get; set; }

    public string? ProcessedBy { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? PaymentReference { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsActive { get; set; }

    public string? Remarks { get; set; }
}
