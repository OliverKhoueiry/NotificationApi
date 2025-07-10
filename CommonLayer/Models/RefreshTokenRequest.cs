using System;

namespace CommonLayer.Models
{
    public class RefreshTokenRequest
    {
        public int UserId { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiry { get; set; }
    }
}
