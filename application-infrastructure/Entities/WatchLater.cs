using System;
using Microsoft.AspNetCore.Identity;

namespace application_infrastructure.Entities;

public class WatchLater
{
    public string? Id { get; set; }
    public string? UserName { get; set; }
    public string? ProgramId { get; set; }
    public string? ProgramName { get; set; }
}

