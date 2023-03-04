using System.ComponentModel.DataAnnotations;

namespace movie_service.DTOs;

public class UpdateRatingDTO
{
    [Required(ErrorMessage = "{0} is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Program id should be a positive integer")]
    public int ProgramId { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [Range(1, 5, ErrorMessage = "Please enter a value between {1} and {2}")]
    public double RatingValue { get; set; }
}
