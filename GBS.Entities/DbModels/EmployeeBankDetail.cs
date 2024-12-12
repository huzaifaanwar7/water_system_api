using System;
using System.Collections.Generic;

namespace GBS.Entities.DbModels;

public partial class EmployeeBankDetail
{
    public int Id { get; set; }

    public int EmployeeIdFk { get; set; }

    public string? BankName { get; set; }

    public string? AccountTitle { get; set; }

    public string? BranchCode { get; set; }

    public string? AccountNumber { get; set; }

    public string? Iban { get; set; }

    public bool IsActive { get; set; }

    public virtual Employee EmployeeIdFkNavigation { get; set; } = null!;
}
