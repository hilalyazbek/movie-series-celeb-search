namespace movie_service.DTOs;

public class MovieDTO
{
    public string? Title { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public long? Revenue { get; set; }

    public double? VoteAverage { get; set; }

    public string? Overview { get; set; }
}
