using System;
using System.ComponentModel.DataAnnotations;

namespace user_details_service.DTOs;

public class RegistrationRequestDTO
{
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Username { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}

