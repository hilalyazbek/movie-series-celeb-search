using System;
using Microsoft.AspNetCore.Identity;

namespace movie_service.DTOs;

public class DeleteWatchLaterDTO
{
    public string? UserId { get; set; }

    public int ProgramId { get; set; }
}

