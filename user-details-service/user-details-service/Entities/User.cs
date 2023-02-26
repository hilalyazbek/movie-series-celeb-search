using System;
using Microsoft.AspNetCore.Identity;

namespace user_details_service.Entities;

public class User : IdentityUser
{
    public string QidNumber { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;
}

