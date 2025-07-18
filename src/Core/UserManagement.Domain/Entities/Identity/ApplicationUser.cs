﻿using Microsoft.AspNetCore.Identity;
using UserManagement.Domain.Common.Interfaces;
using UserManagement.Shared.Kernel.Enums;

namespace UserManagement.Domain.Entities.Identity;

public class ApplicationUser : IdentityUser<Guid>, IAuditableEntity
{
    public UserRole Role { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? PasswordChangedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public string? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public bool IsRefreshTokenRevoked { get; set; }

    public ApplicationUser()
    {

    }

    public ApplicationUser(string userName, string email, string passwordHash, UserRole role)
    {
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        PasswordChangedAt = DateTime.UtcNow;
    }

    public void Deactivate(string modifiedBy)
    {
        IsActive = false;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Activate(string modifiedBy)
    {
        IsActive = true;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void SoftDelete(string deletedBy)
    {
        IsDeleted = true;
        DeletedBy = deletedBy;
        DeletedAt = DateTime.UtcNow;
    }

    public void Restore(string modifiedBy)
    {
        IsDeleted = false;
        DeletedBy = null;
        DeletedAt = null;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }
}
