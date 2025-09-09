using System;
using System.Collections.Generic;

namespace GBS.Entities.DbModels;

public partial class Employee
{
    public int Id { get; set; }

    public int EmployeeCode { get; set; }

    public string Username { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Cnic { get; set; } = null!;

    public string? PersonalEmail { get; set; }

    public Guid? ProfilePictureId { get; set; }

    public string PersonalPhone { get; set; } = null!;

    public string? EmergencyPhone { get; set; }

    public string Password { get; set; } = null!;

    public int StatusIdFk { get; set; }

    public DateTime JoiningDate { get; set; }

    public DateTime? SeparationDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<EmployeeBankDetail> EmployeeBankDetails { get; set; } = new List<EmployeeBankDetail>();

    public virtual ICollection<EmployeeJobRole> EmployeeJobRoles { get; set; } = new List<EmployeeJobRole>();

    public virtual ICollection<EmployeeTechStack> EmployeeTechStacks { get; set; } = new List<EmployeeTechStack>();

    public virtual ICollection<EmployeeUserRole> EmployeeUserRoles { get; set; } = new List<EmployeeUserRole>();

    public virtual Status StatusIdFkNavigation { get; set; } = null!;
}
