using application_infrastructure.Entities;
using application_infrastructure.PagingAndSorting;

namespace application_infrastructure.Repositories.Interfaces;

public interface IWatchLaterRepository : IGenericRepository<WatchLater>
{
    IEnumerable<WatchLater> GetWatchListByUserId(string userId);
    WatchLater AddToWatchLater(WatchLater watchLater);
}
