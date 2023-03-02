using System;
namespace user_details_service.DTOs;

public class ViewUserDTO
{
    public string? Id { get; set; }

    public string? QidNumber { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Username { get; set; }
}

