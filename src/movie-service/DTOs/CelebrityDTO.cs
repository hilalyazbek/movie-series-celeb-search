using TMDbLib.Objects.Search;

namespace movie_service.DTOs;

public class CelebrityDTO
{
    public string? Name { get; set; }

    public DateTime? Birthday { get; set; }

    public string? PlaceOfBirth { get; set; }

    public string? Biography { get; set; }
}
