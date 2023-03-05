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

public class WatchLaterRepository : GenericRepository<WatchLater>, IWatchLaterRepository
{
    public WatchLaterRepository(ApplicationDbContext repositoryContext)
        : base(repositoryContext)
    {
    }

    /// <summary>
    /// This function adds a new watch later item to the database
    /// </summary>
    /// <param name="WatchLater">The object that you want to add to the database.</param>
    /// <returns>
    /// The watchLater object is being returned.
    /// </returns>
    public WatchLater AddToWatchLater(WatchLater watchLater)
    {
        return Create(watchLater);
    }

    /// <summary>
    /// Delete the watch later object from the database
    /// </summary>
    /// <param name="WatchLater">The object that you want to delete from the database.</param>
    /// <returns>
    /// A boolean value.
    /// </returns>
    public bool DeleteFromWatchLater(WatchLater watchLater)
    {
        Delete(watchLater);

        return true;
    }

    /// <summary>
    /// > This function returns a list of all the watch later items for a given user
    /// </summary>
    /// <param name="userId">The userId of the user who's watch list we want to retrieve.</param>
    /// <returns>
    /// A list of WatchLater objects.
    /// </returns>
    public async Task<IEnumerable<WatchLater>> GetWatchListByUserIdAsync(string userId)
    {
        return await FindByCondition(itm => itm.UserId == userId).ToListAsync();
    }

    /// <summary>
    /// > Finds the first item in the watch list that matches the userId and programId
    /// </summary>
    /// <param name="userId">The user's id</param>
    /// <param name="programId">int</param>
    /// <returns>
    /// A WatchLater object.
    /// </returns>
    public async Task<WatchLater>? FindItemInWatchListAsync(string userId, int programId)
    {
        return await FindByCondition(itm => itm.UserId == userId && itm.ProgramId == programId).FirstOrDefaultAsync();
    }

    /// <summary>
    /// This function returns true if the user has a watch list, otherwise it returns false.
    /// </summary>
    /// <param name="userId">The user's id</param>
    /// <returns>
    /// A boolean value.
    /// </returns>
    public async Task<bool> UserHasWatchListAsync(string userId)
    {
        var result = await FindByCondition(itm => itm.UserId == userId).FirstOrDefaultAsync();

        return result is not null;
    }
}