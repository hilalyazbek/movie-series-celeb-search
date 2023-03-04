using System;
using System.ComponentModel.DataAnnotations;

namespace user_details_service.DTOs;

public class UpdateUserDTO
{
    public string QidNumber { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;
}

