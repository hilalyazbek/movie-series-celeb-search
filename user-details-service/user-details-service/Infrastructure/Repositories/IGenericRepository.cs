using System.Linq.Expressions;

namespace user_details_service.Infrastructure.Repositories;

public interface IGenericRepository<T>
{
    IQueryable<T> FindAll();
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    T Create(T entity);
    T Update(T entity);
    void Delete(T entity);
    void Save();
}