using System.ComponentModel.DataAnnotations;

public class CreateUserDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public required string Password { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Rol { get; set; }
}
