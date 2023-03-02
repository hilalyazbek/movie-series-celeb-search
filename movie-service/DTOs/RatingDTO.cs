namespace movie_service.DTOs;

public class RatingDTO
{
    public string? UserId { get; set; }

    public int ProgramId { get; set; }

    public double Rating { get; set; }
}
