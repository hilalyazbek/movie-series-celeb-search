using Microsoft.EntityFrameworkCore;
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

    public async Task<User> GetUserByIdAsync(string id)
    {
        return await FindByCondition(itm => itm.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<User>> GetUsersAsync(UserParameters userParameters)
    {
        return await FindAll()
            .OrderBy(o => o.FirstName)
            .Skip((userParameters.PageNumber - 1) * userParameters.PageSize)
            .Take(userParameters.PageSize)
            .ToListAsync();
    }
}