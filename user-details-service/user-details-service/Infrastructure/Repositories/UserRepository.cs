using Microsoft.EntityFrameworkCore;
using System.Fabric.Query;
using System.Linq.Expressions;
using user_details_service.Entities;
using user_details_service.Infrastructure.DBContexts;

namespace user_details_service.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext repositoryContext) 
        : base(repositoryContext)
    {
    }

    public IEnumerable<User> GetUsers(UserParameters userParameters)
    {
        return FindAll()
            .OrderBy(o => o.FirstName)
            .Skip((userParameters.PageNumber - 1) * userParameters.PageSize)
            .Take(userParameters.PageSize)
            .ToList();
    }
}