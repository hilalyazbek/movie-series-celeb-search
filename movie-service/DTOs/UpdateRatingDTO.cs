namespace movie_service.DTOs;

public class UpdateRatingDTO
{
    public int ProgramId { get; set; }

    public double RatingValue { get; set; }
}
