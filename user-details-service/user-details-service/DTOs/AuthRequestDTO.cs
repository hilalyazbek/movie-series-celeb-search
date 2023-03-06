using System;
namespace user_details_service.DTOs;

public class AuthRequestDTO
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

