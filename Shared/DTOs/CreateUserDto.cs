using System.ComponentModel.DataAnnotations;

namespace DTOs;

public class CreateUserDto
{
    [Required] // Makes sure the client can't send a null/empty username
    public string UserName { get; set; }

    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")] // Enforces password rules
    public string Password { get; set; }
}