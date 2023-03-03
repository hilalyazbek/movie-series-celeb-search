using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;
using application_infrastructure.DBContexts;
using application_infrastructure.Entities;
using application_infrastructure.PagingAndSorting;
using application_infrastructure.Repositories.Interfaces;

namespace application_infrastructure.Repositories.Implementations;

public class RatingRepository : GenericRepository<Rating>, IRatingRepository
{
    public RatingRepository(ApplicationDbContext repositoryContext)
        : base(repositoryContext)
    {
    }

    public Rating? GetRatingByProgramID(int programId)
    {
        return FindByCondition(itm => itm.ProgramId == programId).FirstOrDefault();
    }

    public IEnumerable<Rating> GetRatings()
    {
        return FindAll();
    }

    public Rating RateMovie(Rating rating)
    {
        var ratingInDatabase = FindByCondition(itm => itm.ProgramId == rating.ProgramId).FirstOrDefault();
        if (ratingInDatabase is null)
        {
            rating.RatedBy += 1;
            return Create(rating);
        }

        var newRating = (ratingInDatabase.RatingValue + rating.RatingValue) / 2;
        ratingInDatabase.RatingValue = newRating;
        ratingInDatabase.RatedBy += 1;

        return Update(ratingInDatabase);
    }
}