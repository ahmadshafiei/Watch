using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Watch.DataAccess.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public readonly DbContext dbContext;
        public readonly DbSet<T> dbSet;

        public GenericRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public void DeleteById(int Id)
        {
            dbSet.Remove(dbSet.Find(Id));
        }

        public IQueryable<T> Get()
        {
            return dbSet.AsQueryable();
        }

        public List<T> GetAll()
        {
            return dbSet.ToList();
        }

        public List<T> GetAll(Expression<Func<T, object>> include)
        {
            IQueryable<T> result = dbSet;
            result = result.Include(include);
            result.Include(w => w);
            return result.ToList();
        }

        public List<T> GetAll(out int count, Expression<Func<T, bool>> where, int? skip, int? take, Expression<Func<T, int>> orderBy, Expression<Func<T, object>> include)
        {
            IQueryable<T> result = dbSet;

            if (where != null)
                result = result.Where(where);

            if (orderBy != null)
                result = result.OrderBy(orderBy);

            count = result.Count();

            if (skip != null)
                result = result.Skip((int)skip);
            if (take != null)
                result = result.Take((int)take);

            if (include != null)
                result = result.Include(include);

            return result.ToList();
        }

        public T GetById(int Id)
        {
            return dbSet.Find(Id);
        }

        public void Insert(T entity)
        {
            dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
