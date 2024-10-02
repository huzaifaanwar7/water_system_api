using System;
using System.ComponentModel.DataAnnotations;

namespace PrescottAppBackend.Domain
{
    public class UserVM
    {
        [Key]
        public string? Id { get; set; }
        public string? RoleId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public bool EmailVerified { get; set; } = false;
        public string? Password { get; set; }
        public string? Mobile { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? FirebaseId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string? UserSignUpType { get; set; }
        public string? BusinessName { get; set; }
        public string? BusinessType { get; set; }

        public enum UserType
        {
            Email,
            Google,
            Microsoft,
            Apple
        }

    }
}
