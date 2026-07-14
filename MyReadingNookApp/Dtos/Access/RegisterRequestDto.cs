using System.ComponentModel.DataAnnotations;

namespace MyReadingNookApp.Dtos;

public class RegisterRequestDto()
{
    [Required]
    [MaxLength(20)]
    public string Username { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [MinLength(8)]
    public string Password { get; set; }
}