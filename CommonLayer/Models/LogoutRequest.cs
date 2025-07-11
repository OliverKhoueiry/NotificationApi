using System.ComponentModel.DataAnnotations;

public class LogoutRequest
{
    [Required(ErrorMessage = "Refresh token is required.")]
    public string RefreshToken { get; set; }
}
