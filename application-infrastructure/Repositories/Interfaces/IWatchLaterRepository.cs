using application_infrastructure.Entities;
using application_infrastructure.PagingAndSorting;

namespace application_infrastructure.Repositories.Interfaces;

public interface IWatchLaterRepository : IGenericRepository<WatchLater>
{
    WatchLater AddToWatchLater(WatchLater watchLater);
}
