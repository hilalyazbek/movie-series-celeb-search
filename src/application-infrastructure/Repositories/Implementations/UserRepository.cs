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

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext repositoryContext) 
        : base(repositoryContext)
    {
    }

    /// <summary>
    /// > Get the first user from the database that has an Id that matches the Id passed in
    /// </summary>
    /// <param name="id">The id of the user to retrieve</param>
    /// <returns>
    /// A user object
    /// </returns>
    public async Task<User> GetUserByIdAsync(string id)
    {
        return await FindByCondition(itm => itm.Id == id).FirstOrDefaultAsync();
    }

    /// <summary>
    /// > Get all users, sort them, and then return a page of them
    /// </summary>
    /// <param name="PagingParameters"></param>
    /// <param name="SortingParameters"></param>
    /// <returns>
    /// A list of users.
    /// </returns>
    public async Task<IEnumerable<User>> GetUsersAsync(PagingParameters pagingParameters,
         SortingParameters sortingParameters)
    {
        var users = FindAll();

        ApplySort(ref users, sortingParameters.SortBy);

        return await users
            .Skip((pagingParameters.PageNumber - 1) * pagingParameters.PageSize)
            .Take(pagingParameters.PageSize)
            .ToListAsync();
    }

    /// <summary>
    /// It takes a queryable collection of users and an orderByQueryString, and returns the same
    /// queryable collection of users, but sorted according to the orderByQueryString
    /// </summary>
    /// <param name="users">IQueryable<User></param>
    /// <param name="orderByQueryString">This is the string that is passed in the query string.</param>
    /// <returns>
    /// A list of users.
    /// </returns>
    private void ApplySort(ref IQueryable<User> users, string orderByQueryString)
    {
        if (!users.Any())
            return;

        if (string.IsNullOrWhiteSpace(orderByQueryString))
        {
            users = users.OrderBy(x => x.FirstName);
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
            users = users.OrderBy(x => x.FirstName);
            return;
        }
        users = users.OrderBy(orderQuery);
    }
}