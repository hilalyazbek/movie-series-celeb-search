using System;
using Microsoft.AspNetCore.Identity;

namespace movie_service.DTOs;

public class CreateWatchLaterDTO
{
    public Guid Id { get; set; }

    public string? UserId { get; set; }

    public int ProgramId { get; set; }

    public string? ProgramName { get; set; }
}

