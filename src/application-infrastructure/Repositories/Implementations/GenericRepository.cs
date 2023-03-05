using application_infrastructure.DBContexts;
using application_infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace application_infrastructure.Repositories.Implementations;

/* It's a generic repository that implements the IGenericRepository interface */
public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected ApplicationDbContext context { get; set; }

    /* It's a generic repository that implements the IGenericRepository interface */
    public GenericRepository(ApplicationDbContext repositoryContext)
    {
        this.context = repositoryContext;
    }

    public async Task<IQueryable<T>> FindAll()
    {
        return await this.context.Set<T>()
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

        Save();

        return result.Entity;
    }

    public T Update(T entity)
    {
        var result = context.Set<T>().Update(entity);
        
        Save();

        return result.Entity;

    }

    public void Delete(T entity)
    {
        context.Set<T>().Remove(entity);

        Save();
    }

    public void Save()
    {
        context.SaveChanges();
    }
}