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

    /// <summary>
    /// This function returns the rating that matches the program ID
    /// </summary>
    /// <param name="programId">The id of the program that the rating is for</param>
    /// <returns>
    /// A Rating object
    /// </returns>
    public Rating? GetRatingByProgramID(int programId)
    {
        return FindByCondition(itm => itm.ProgramId == programId).FirstOrDefault();
    }

    /// <summary>
    /// It returns all the ratings in the database
    /// </summary>
    /// <returns>
    /// A list of all the ratings in the database.
    /// </returns>
    public IEnumerable<Rating> GetRatings()
    {
        return FindAll();
    }

    /// <summary>
    /// It takes a rating object, checks if it exists in the database, if it does, it updates the rating
    /// value and the number of people who rated it, if it doesn't, it creates a new rating object
    /// </summary>
    /// <param name="Rating">The rating object that is passed in from the frontend.</param>
    /// <returns>
    /// The rating object is being returned.
    /// </returns>
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