using System.Fabric.Query;
using user_details_service.Entities;

namespace user_details_service.Infrastructure.Repositories;

public interface IUserRepository:IGenericRepository<User>
{
    IEnumerable<User> GetUsers(UserParameters userParameters);
}
