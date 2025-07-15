namespace CommonLayer.Models
{
    public class LoginResponse
    {
        public string Description { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; } 
    }
}
