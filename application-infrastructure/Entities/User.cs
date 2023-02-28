using System;
using Microsoft.AspNetCore.Identity;

namespace application_infrastructure.Entities;

public class User : IdentityUser
{
    public string QidNumber { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;
}

