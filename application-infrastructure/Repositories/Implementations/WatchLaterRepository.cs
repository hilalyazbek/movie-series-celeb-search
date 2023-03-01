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
        var created = Create(watchLater);
        Save();
        return created;
        
    }
}