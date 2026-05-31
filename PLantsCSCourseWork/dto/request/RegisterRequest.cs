using System.ComponentModel.DataAnnotations;

namespace Gardener.dto.request;

public class RegisterRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;
    [Required, Compare(nameof(Password))]
    public string PasswordCheck { get; set; } = string.Empty;
    [Required, Phone]
    public string Phone { get; set; } = string.Empty;
    public string? Nickname { get; set; }
}