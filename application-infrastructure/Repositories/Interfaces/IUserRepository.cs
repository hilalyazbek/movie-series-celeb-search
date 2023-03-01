using application_infrastructure.Entities;
using application_infrastructure.PagingAndSorting;

namespace application_infrastructure.Repositories.Interfaces;

public interface IUserRepository:IGenericRepository<User>
{
    Task<IEnumerable<User>> GetUsersAsync(PagingParameters userParameters, SortingParameters sortingParameters);
    Task<User> GetUserByIdAsync(string id);
}
