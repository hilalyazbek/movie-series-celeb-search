using System;
namespace user_details_service.DTOs;

public class ViewUserDTO
{
    public string QidNumber { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Username { get; set; } = null!;
}

