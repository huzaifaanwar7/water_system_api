using System;
using System.ComponentModel.DataAnnotations;

namespace GBS.Model
{
    public class EmployeePM
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PersonalPhone { get; set; }
        public Guid? ProfilePictureId { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Cnic { get; set; }
        public string? PersonalEmail { get; set; }
        public string? Username { get; set; }
        public IEnumerable<int>? JobRole { get; set; }
        public IEnumerable<int>? UserRole { get; set; }
        public IEnumerable<int>? TechStack { get; set; }
        public int Status { get; set; }
    }

     public class EmployeeUpdatePM
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PersonalPhone { get; set; }
        public Guid? ProfilePictureId { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Cnic { get; set; }
        public string? PersonalEmail { get; set; }
        // public string? Username { get; set; }
        public IEnumerable<int>? JobRole { get; set; }
        public IEnumerable<int>? UserRole { get; set; }
        public IEnumerable<int>? TechStack { get; set; }
        public int Status { get; set; }
    }  
    
    public class ProfessionalDetailsPM
    {
        public int Id { get; set; }
        public DateTime JoiningDate { get; set; }
        // public string? Username { get; set; }
        public IEnumerable<int>? JobRole { get; set; }
        public IEnumerable<int>? UserRole { get; set; }
        public IEnumerable<int>? TechStack { get; set; }
        public int? Status { get; set; }
        public DateTime? SeparationDate { get; set; }
    }

    public class PersonalDetailsPM
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PersonalPhone { get; set; }
        public Guid? ProfilePictureId { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Cnic { get; set; }
        public string? PersonalEmail { get; set; }
        // public string? Username { get; set; }
       
    }
}
