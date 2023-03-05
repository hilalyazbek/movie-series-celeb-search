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

    /// <summary>
    /// Get all the search history, sort it, and then return the page of results.
    /// </summary>
    /// <param name="PagingParameters"></param>
    /// <param name="SortingParameters"></param>
    /// <returns>
    /// A list of SearchHistory objects.
    /// </returns>
    public async Task<IEnumerable<SearchHistory>> GetAllAsync(PagingParameters pagingParameters, SortingParameters sortingParameters)
    {
        var history = FindAll();

        ApplySort(ref history, sortingParameters.SortBy);

        return await history
            .Skip((pagingParameters.PageNumber - 1) * pagingParameters.PageSize)
            .Take(pagingParameters.PageSize)
            .ToListAsync();
    }

    /// <summary>
    /// It creates a new search history record and returns all search history records
    /// </summary>
    /// <param name="SearchHistory">This is the model that I'm using to save the data.</param>
    /// <returns>
    /// IEnumerable<SearchHistory>
    /// </returns>
    public IEnumerable<SearchHistory> Save(SearchHistory search)
    {
        Create(search);

        return FindAll();
    }

    /// <summary>
    /// It takes a queryable collection of SearchHistory objects and an orderByQueryString parameter,
    /// and returns the same queryable collection of SearchHistory objects, but sorted according to the
    /// orderByQueryString parameter
    /// </summary>
    /// <param name="history">IQueryable<SearchHistory></param>
    /// <param name="orderByQueryString">"Query desc"</param>
    /// <returns>
    /// IQueryable<SearchHistory>
    /// </returns>
    private static void ApplySort(ref IQueryable<SearchHistory> history, string orderByQueryString)
    {
        if (!history.Any())
            return;

        if (string.IsNullOrWhiteSpace(orderByQueryString))
        {
            history = history.OrderBy(x => x.Query);
            return;
        }

        var orderParams = orderByQueryString.Trim().Split(',');
        var propertyInfos = typeof(User).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var orderQueryBuilder = new StringBuilder();
        foreach (var param in orderParams)
        {
            if (string.IsNullOrWhiteSpace(param))
                continue;
            var propertyFromQueryName = param.Split(" ")[0];
            var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
            if (objectProperty == null)
                continue;
            var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";
            orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");
        }
        var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
        if (string.IsNullOrWhiteSpace(orderQuery))
        {
            history = history.OrderBy(x => x.Query);
            return;
        }
        history = history.OrderBy(orderQuery);
    }
}