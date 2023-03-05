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

    public async Task<IEnumerable<WatchLater>> GetWatchListByUserIdAsync(string userId)
    {
        return await FindByCondition(itm => itm.UserId == userId).ToListAsync();
    }

    public async Task<WatchLater>? FindItemInWatchListAsync(string userId, int programId)
    {
        return await FindByCondition(itm => itm.UserId == userId && itm.ProgramId == programId).FirstOrDefaultAsync();
    }

    public async Task<bool> UserHasWatchListAsync(string userId)
    {
        var result = await FindByCondition(itm => itm.UserId == userId).FirstOrDefaultAsync();

        return result is not null;
    }
}