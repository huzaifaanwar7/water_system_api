using System;
using System.Collections.Generic;

namespace GBS.Entities.DbModels;

public partial class User
{
    public int Id { get; set; }

    public int EmployeeCode { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Cnic { get; set; } = null!;

    public string? PersonalEmail { get; set; }

    public string PersonalPhone { get; set; } = null!;

    public string? EmergencyPhone { get; set; }

    public string Password { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Role { get; set; } = null!;

    public int StatusFk { get; set; }

    public string? TechStack { get; set; }

    public DateTime JoiningDate { get; set; }

    public DateTime? SeparationDate { get; set; }

    public string? BankName { get; set; }

    public string? AccountTitle { get; set; }

    public string? BranchCode { get; set; }

    public string? AccountNumber { get; set; }

    public string? Iban { get; set; }

    public bool IsActive { get; set; }

    public virtual Lookup StatusFkNavigation { get; set; } = null!;
}
