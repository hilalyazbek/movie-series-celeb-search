using System.ComponentModel.DataAnnotations;

namespace movie_service.DTOs;

public class ViewRatingDTO
{
    public int ProgramId { get; set; }

    public double RatingValue { get; set; }

    public int RatedBy { get; set; }
}
