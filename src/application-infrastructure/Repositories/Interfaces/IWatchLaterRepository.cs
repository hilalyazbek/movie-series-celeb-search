using application_infrastructure.Entities;
using application_infrastructure.PagingAndSorting;

namespace application_infrastructure.Repositories.Interfaces;

public interface IWatchLaterRepository : IGenericRepository<WatchLater>
{
    IEnumerable<WatchLater> GetWatchListByUserId(string userId);
    WatchLater AddToWatchLater(WatchLater watchLater);
    bool DeleteFromWatchLater(WatchLater watchLater);
    bool UserHasWatchList(string userId);
    WatchLater? FindItemInWatchList(string userId, int programId);
}
