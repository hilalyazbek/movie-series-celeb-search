using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;

namespace movie_service.DTOs;

public class CreateWatchLaterDTO
{
    [Required(ErrorMessage = "{0} is required")]
    [RegularExpression("^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", ErrorMessage = "{0} is not a valid GUID format")]
    public string? UserId { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Program id should be a positive integer")]
    public int ProgramId { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    public string? ProgramName { get; set; }
}

