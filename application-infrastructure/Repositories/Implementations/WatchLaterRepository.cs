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

    public WatchLater AddToWatchLater(WatchLater watchLater)
    {
        return Create(watchLater);
    }

    public bool DeleteFromWatchLater(WatchLater watchLater)
    {
        Delete(watchLater);

        return true;
    }

    public IEnumerable<WatchLater> GetWatchListByUserId(string userId)
    {
        return FindByCondition(itm => itm.UserId == userId);
    }

    public WatchLater? FindItemInWatchList(string userId, int programId)
    {
        return FindByCondition(itm => itm.UserId == userId && itm.ProgramId == programId).FirstOrDefault();
    }

    public bool UserHasWatchList(string userId)
    {
        var result = FindByCondition(itm => itm.UserId == userId).FirstOrDefault();

        return result is not null ? true : false;
    }
}