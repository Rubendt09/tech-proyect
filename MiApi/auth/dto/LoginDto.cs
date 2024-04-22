using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;
}