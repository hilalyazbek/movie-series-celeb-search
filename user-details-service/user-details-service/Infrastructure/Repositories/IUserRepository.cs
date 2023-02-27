using user_details_service.Entities;

namespace user_details_service.Infrastructure.Repositories;

public interface IUserRepository:IGenericRepository<User>
{
    Task<IEnumerable<User>> GetUsersAsync(UserParameters userParameters);
    Task<User> GetUserByIdAsync(string id);
}
