using System;
using Microsoft.AspNetCore.Identity;

namespace application_infrastructure.Entities;

public class User : IdentityUser
{
    public string? QidNumber { get; set; }

    public string? FirstName { get; set; } 

    public string? LastName { get; set; }
}

