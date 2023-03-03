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

public class SearchHistoryRepository : GenericRepository<SearchHistory>, ISearchHistoryRepository
{
    public SearchHistoryRepository(ApplicationDbContext repositoryContext)
        : base(repositoryContext)
    {
    }

    public IEnumerable<SearchHistory> Save(SearchHistory search)
    {
        var created = Create(search);

        return FindAll();
    }
}