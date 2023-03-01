namespace movie_service.DTOs;

public class TVShowDTO
{
    public string Name { get; set; } = null!;

    public DateTime? FirstAirDate { get; set; }

    public int? NumberOfSeasons { get; set; }

    public int? NumberOfEpisodes { get; set; }

    public string? Overview { get; set; }
}
