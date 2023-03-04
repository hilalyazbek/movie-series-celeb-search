using application_infrastructure.Entities;

namespace application_infrastructure.Repositories.Interfaces;

public interface IRatingRepository : IGenericRepository<Rating>
{
    IEnumerable<Rating> GetRatings();
    Rating? GetRatingByProgramID(int programId);
    Rating RateMovie(Rating rating);
}
