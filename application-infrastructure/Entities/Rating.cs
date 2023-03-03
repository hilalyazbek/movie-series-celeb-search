using System;
using Microsoft.AspNetCore.Identity;

namespace application_infrastructure.Entities;

public class Rating
{
    public Guid Id { get; set; }

    public int ProgramId { get; set; }

    public double RatingValue { get; set; }

    public int RatedBy { get; set; }
}

