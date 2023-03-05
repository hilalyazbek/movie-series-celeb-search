using application_infrastructure.Entities;
using application_infrastructure.PagingAndSorting;

namespace application_infrastructure.Repositories.Interfaces;

public interface IWatchLaterRepository : IGenericRepository<WatchLater>
{
    Task<IEnumerable<WatchLater>> GetWatchListByUserIdAsync(string userId);
    WatchLater AddToWatchLater(WatchLater watchLater);
    bool DeleteFromWatchLater(WatchLater watchLater);
    Task<bool> UserHasWatchListAsync(string userId);
    Task<WatchLater>? FindItemInWatchListAsync(string userId, int programId);
}
