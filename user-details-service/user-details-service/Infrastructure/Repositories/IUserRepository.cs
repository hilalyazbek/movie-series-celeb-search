using user_details_service.Entities;
using user_details_service.Helpers.PagingAndSorting;

namespace user_details_service.Infrastructure.Repositories;

public interface IUserRepository:IGenericRepository<User>
{
    Task<IEnumerable<User>> GetUsersAsync(PagingParameters userParameters, SortingParameters sortingParameters);
    Task<User> GetUserByIdAsync(string id);
}
