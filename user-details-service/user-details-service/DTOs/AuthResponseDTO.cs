using System;
namespace user_details_service.DTOs;

public class AuthResponseDTO
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Token { get; set; }
}

