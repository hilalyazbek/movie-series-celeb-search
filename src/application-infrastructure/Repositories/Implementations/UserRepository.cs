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

    public async Task<User> GetUserByIdAsync(string id)
    {
        return await FindByCondition(itm => itm.Id == id).FirstOrDefaultAsync();
    }

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