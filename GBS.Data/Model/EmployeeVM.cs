using System;
using System.ComponentModel.DataAnnotations;

namespace GBS.Data.Model
{
    public class EmployeeVM
    {
        [Key]
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
