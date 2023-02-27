using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using user_details_service.Infrastructure.DBContexts;

namespace user_details_service.Infrastructure.Repositories;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected ApplicationDbContext context { get; set; }

    public GenericRepository(ApplicationDbContext repositoryContext)
    {
        this.context = repositoryContext;
    }

    public IQueryable<T> FindAll()
    {
        return this.context.Set<T>()
            .AsNoTracking();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
    {
        return context.Set<T>()
            .Where(expression)
            .AsNoTracking();
    }

    public T Create(T entity)
    {
        var result = context.Set<T>().Add(entity);
        return result.Entity;
    }

    public T Update(T entity)
    {
        var result = context.Set<T>().Update(entity);
        return result.Entity;
    }

    public void Delete(T entity)
    {
        context.Set<T>().Remove(entity);
    }

    public void Save()
    {
        context.SaveChanges();
    }
}