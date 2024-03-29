﻿using System;
using Microsoft.AspNetCore.Identity;

namespace application_infrastructure.Entities;

public class WatchLater : BaseEntity
{
    public string? UserId { get; set; }

    public int ProgramId { get; set; }

    public string? ProgramName { get; set; }
}