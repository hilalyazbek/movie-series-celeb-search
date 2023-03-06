using application_infrastructure.Entities;

namespace application_infrastructure.Repositories.Interfaces;

public interface IRatingRepository : IGenericRepository<Rating>
{
    Task<IEnumerable<Rating>> GetRatingsAsync();
    Task<Rating>? GetRatingByProgramIDAsync(int programId);
    Rating RateMovie(Rating rating);
}
