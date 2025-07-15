﻿public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
    public string? ResetToken { get; set; }
    public DateTime? ResetTokenExpiry { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Role { get; set; } = "User"; // Default role
}
