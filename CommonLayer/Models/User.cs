using System;
using System.Collections.Generic;

namespace CommonLayer.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
       // public string Role { get; set; } = "User"; // 👈 Add this (default to "User")
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<UserPermission> Permissions { get; set; } = new List<UserPermission>(); // 👈 Add this too

        public string? ResetToken { get; set; } // 👈 Add this
        public DateTime? ResetTokenExpiry { get; set; } // 👈 Add this
        public string Role { get; set; } = "User"; // Default role

    }
}


//public class UserPermission
//{
//    public string Section { get; set; }
//    public string Action { get; set; }
//}
