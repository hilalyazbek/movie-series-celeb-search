using System.Linq.Expressions;

namespace application_infrastructure.Repositories.Interfaces;

public interface IGenericRepository<T>
{
    IQueryable<T> FindAll();
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    T Create(T entity);
    T Update(T entity);
    void Delete(T entity);
    void Save();
}