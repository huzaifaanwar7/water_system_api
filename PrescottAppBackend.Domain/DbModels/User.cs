using System;
using System.Collections.Generic;

namespace PrescottAppBackend.Domain.DbModels;

public partial class User
{
    public string Id { get; set; } = null!;

    public string RoleId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool EmailVerified { get; set; }

    public string Password { get; set; } = null!;

    public string? Mobile { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string FirebaseId { get; set; } = null!;

    public string? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public bool? IsActive { get; set; }

    public string UserSignUpType { get; set; } = null!;

    public string? BusinessName { get; set; }

    public string? BusinessType { get; set; }
}
