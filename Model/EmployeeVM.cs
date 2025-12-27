using System;
using System.ComponentModel.DataAnnotations;

namespace GBS.Model
{
    public class EmployeeVM
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        //public bool EmailVerified { get; set; } = false;
        public string? PersonalPhone { get; set; }
        public string ProfilePictureUrl { get; set; }
        public Guid? ProfilePictureId { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime? SeparationDate { get; set; }
        public string Cnic { get; set; }
        public string? PersonalEmail { get; set; }
        public string Username { get; set; }
        public IEnumerable<string?> JobRole { get; set; }
        public IEnumerable<string?> UserRole { get; set; }
        public IEnumerable<string?> TechStack { get; set; }
        public string? Status { get; set; }
        public IEnumerable<BankDetailVM> BankDetail { get; set; }
        public IEnumerable<EmployeeLedgerVM> Ledger { get; set; }
    }

    public class EmployeeUpComingNews
    {
        public int ID { get; set; }
        public string Name { get; set; }
        
        public DesignationVM Designation { get; set; }
        public DateTime? Date { get; set; }
        public string DefinitionType { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
   
   public class DesignationVM
    {
       public string Title { get; set;}

       public string Department { get; set;}
    }


    public class EmployeeDto {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        //public bool EmailVerified { get; set; } = false;
        public string? PersonalPhone { get; set; }
        public string ProfilePictureUrl { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime? SeparationDate { get; set; }
        public string Cnic { get; set; }
        public string? PersonalEmail { get; set; }
        public string Username { get; set; }
    }
}
