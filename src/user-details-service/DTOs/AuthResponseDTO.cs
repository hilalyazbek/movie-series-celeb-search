using System;
namespace user_details_service.DTOs;

public class AuthResponseDTO
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;
}

