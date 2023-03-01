using System;
using Microsoft.AspNetCore.Identity;

namespace application_infrastructure.Entities;

public class Rating
{
    public Guid Id { get; set; }

    public int ProgramId { get; set; }

    public string? ProgramName { get; set; }

    public double UserRating { get; set; }
}

