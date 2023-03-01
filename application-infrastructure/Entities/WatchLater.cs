using System;
using Microsoft.AspNetCore.Identity;

namespace application_infrastructure.Entities;

public class WatchLater
{
    public Guid Id { get; set; }

    public User? User { get; set; }

    public string? UserId { get; set; }

    public int ProgramId { get; set; }

    public string? ProgramName { get; set; }
}

