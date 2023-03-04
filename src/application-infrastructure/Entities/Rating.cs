using System;
using Microsoft.AspNetCore.Identity;

namespace application_infrastructure.Entities;

public class Rating:BaseEntity
{
    public int ProgramId { get; set; }

    public double RatingValue { get; set; }

    public int RatedBy { get; set; }
}

